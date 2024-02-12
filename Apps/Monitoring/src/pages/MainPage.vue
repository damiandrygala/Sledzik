<template>
  <q-dialog v-model="firstLaunchAlert">
    <q-card>
      <q-card-section>
        <div class="text-h6">This is your first launch! <q-circular-progress indeterminate rounded size="30px"
            color="lime" class="q-ma-md" /> </div>
      </q-card-section>
      <q-card-section class="q-pt-none">
        In 3 seconds you will be moved to manual page to learn how to use that system.
      </q-card-section>
      <q-card-actions align="right">
        <q-btn flat label="OK" color="primary" v-close-popup />
      </q-card-actions>
    </q-card>
  </q-dialog>
  <q-drawer side="right" v-model="violationBar" bordered :width="600" :breakpoint="500">
    <q-scroll-area class="fit">
      <div class="q-pa-sm">
        <div class="text-h5" align="center">Violation status</div><br /><br /><br />
        <div v-if="currentViolation.length == 0" class="text-h6" align="center"> No violations </div>
        <div v-for="dev in currentViolation" :key="dev" align="center">
          <q-btn-dropdown split color="red" push no-caps icon="smartphone" :label="violatedDeviceLabel(dev)"
            @click="onMainClick(dev)">
            <q-list>
              <div v-for="zone in dev.violation" :key="zone">
                <q-item clickable v-close-popup @click="onItemClick(dev, zone)">
                  <q-item-section avatar>
                    <q-avatar icon="map" color="primary" text-color="white" />
                  </q-item-section>
                  <q-item-section>
                    <q-item-label>{{ zone.name }}</q-item-label>
                  </q-item-section>
                </q-item>
              </div>
            </q-list>
          </q-btn-dropdown>
        </div>
      </div>
    </q-scroll-area>
  </q-drawer>
  <q-page padding class="flex flex-center bg-grey-3">
    <div class="h-screen relative">
      <h4>Welcome to Monitoring application</h4>
      <q-select clearable v-model="selectedObject" :options="ohObjects" label="Select Tracker Object to display"
        @update:model-value="selectedObjectUpdate" />
      <br />
      <div v-if="!isObjectSelected">
        <q-select disable clearable v-model="selectedZone" :options="ohZones" label="Select zone to display"
          hint="First you need to select an object" />
      </div>
      <div v-if="isObjectSelected">
        <q-select clearable multiple v-model="selectedZone" :options="ohZones" label="Select zone to display"
          @update:model-value="selectedZonesUpdate" />
      </div>
      <br />
      <br />
      <DisplayMapVue :zoneCoords="zoneCoords" :trackerCoords="trackerCoords" />
    </div>
  </q-page>
</template>

<script>
import { useObjectObservedStore } from "src/stores/object-observed";
import { defineComponent } from "vue";
import { onMounted, watch, ref } from "vue";
import { urlApi } from "src/additional-configuration";
import DisplayMapVue from "src/components/DisplayMap-IRT.vue"
import { useRouter } from "vue-router";

export default defineComponent({
  name: "IndexPage",
  components: {
    DisplayMapVue
  },
  setup() {
    const firstLaunchAlert = ref(false);
    if (document.cookie != 'NotFirstLaunch=true') {
      firstLaunchAlert.value = true;
      const router = useRouter();
      setTimeout(() => {
        document.cookie = 'NotFirstLaunch=true; expires=' + new Date(9999, 0, 1).toUTCString();
        router.push('/readme');
      }, 3000)
    }

    // === TEMPLATE VARIABLES ===
    const ohObjects = ref([]);
    const ohZones = ref([]);
    const selectedZone = ref([]);
    const selectedObject = ref(null);
    const isObjectSelected = ref(false);
    const axios = require('axios');
    // === TEMPLATE VARIABLES ===

    const zoneCoords = ref([]);

    // === DEVICES ===
    let devices = null;
    let tmpZones = null;
    // === DEVICES ===

    // === TRACKER COORDS ===
    const selectedObjectId = ref("");
    const trackerCoords = ref([0, 0]);
    const store = useObjectObservedStore();
    const mapIsLoading = ref(true);
    // === TRACKER COORDS ===

    trackerCoords.value = store.objectObservedForWatch;

    onMounted(async () => {
      // DEVICES
      const getAllObjects = await axios.get(`${urlApi}/device?NamedOnly=true`);
      const getAllZonesRequest = await axios.get(`${urlApi}/Zone`);
      devices = getAllObjects;
      tmpZones = getAllZonesRequest;
      showRegObjects(devices);
    });

    const showRegObjects = (devices) => {
      if (!!devices) {
        devices.data.deviceItems.forEach(dev => {
          if (!!dev.name) ohObjects.value.push(dev.name);
        });
      }
      else console.log("There is no devices from API")
    }

    const selectedObjectUpdate = async () => {
      ohZones.value = [];
      selectedZone.value = [];

      if (!!selectedObject.value) {
        devices.data.deviceItems.forEach(dev => {
          if (dev.name == selectedObject.value) selectedObjectId.value = dev.id
        });
        const getAssignedZones = await axios.get(`${urlApi}/zone/spec?AssignedToDeviceId=${selectedObjectId.value}`);
        getAssignedZones.data.zones.forEach(zone => {
          ohZones.value.push(zone.zoneName);
          selectedZone.value.push(zone.zoneName);
        })
        displayZonesDetails(getAssignedZones.data.zones, selectedZone.value)
        isObjectSelected.value = true;
        mapIsLoading.value = false;
      }
      else {
        isObjectSelected.value = false;
        displayZonesDetails();
      }
    }
    const selectedZonesUpdate = async () => {
      if (!!selectedZone.value) {
        const getAssignedZones = await axios.get(`${urlApi}/zone/spec?AssignedToDeviceId=${selectedObjectId.value}`);
        displayZonesDetails(getAssignedZones.data.zones, selectedZone.value);
      }
      else displayZonesDetails();
    }

    const displayZonesDetails = (zones, constraint) => {
      if (!!zones) {
        const tmpZoneCoords = ref([]);
        zones.forEach(zone => {
          if (constraint.some(d => d == zone.zoneName)) tmpZoneCoords.value.push(zone.coordinates);
        })
        zoneCoords.value = tmpZoneCoords.value
      }
      else zoneCoords.value = [];
    }

    watch(trackerCoords, (newValue, oldValue) => {
      writeViolation(newValue);
    }, { deep: true })

    const currentViolation = ref([]);
    const writeViolation = (violation) => {
      const indexOfViolation = currentViolation.value.findIndex(v => v.deviceId == violation.deviceId);
      if (indexOfViolation != -1) {
        let tmpViolation = [];
        violation.violation.forEach(id => {
          tmpViolation.push({ id: id, name: getViolatedZoneName(id) })
        })
        currentViolation.value.at(indexOfViolation).violation = tmpViolation
      }
      else {
        let tmpViolation = [];
        violation.violation.forEach(id => {
          tmpViolation.push({ id: id, name: getViolatedZoneName(id) })
        })
        currentViolation.value.push({
          deviceId: violation.deviceId,
          deviceName: getDeviceName(violation.deviceId),
          violation: tmpViolation,
        });
      }
    }

    const getDeviceName = (id) => {
      return devices.data.deviceItems.find(d => d.id == id).name;
    }
    const getViolatedZoneName = (id) => {
      return tmpZones.data.zones.find(z => z.id == id).zoneName;
    }

    const violatedDeviceLabel = (n) => {
      return n.deviceName;
    }

    const onMainClick = (device) => {
      selectedObject.value = device.name;
      selectedObjectUpdate();
    }

    const onItemClick = async (device, zone) => {
      selectedObject.value = device.name;
      await selectedObjectUpdate();
      selectedZone.value = [zone.zoneName];
      selectedZonesUpdate();
    }

    return {
      firstLaunchAlert,
      trackerCoords,
      selectedZone,
      ohZones,
      ohObjects,
      selectedObject,
      isObjectSelected,
      selectedObjectUpdate,
      selectedZonesUpdate,
      zoneCoords,
      violationBar: ref(true),
      violatedDeviceLabel,
      currentViolation,
      onMainClick,
      onItemClick,
    }
  },
});
</script>
