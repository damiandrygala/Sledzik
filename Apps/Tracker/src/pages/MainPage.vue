<template>
  <div>
    <q-layout view="lHh Lpr lFf">
      <ControlPanel :violatedZones="violationStatus" />
      <q-page-container>
        <LogoIcon />
        <br /><br />
        <MapArea :trackerCoords="trackerCoords" :zones="zonesData" />
        Last update timestamp: {{ lastUpdateTimestamp }}
        <br /><br />
        <TrackingActivation @buttonStatus="activationState" />
        <GeoLocation :activation="activation" @positionChanged="positionChanged" />
      </q-page-container>
    </q-layout>
  </div>
</template>

<script>
import GeoLocation from 'src/components/GeoLocation/GeoLocation.vue';
import ControlPanel from 'src/components/MainPageLayout/ControlPanel.vue';
import LogoIcon from 'src/components/MainPageLayout/LogoIcon.vue';
import MapArea from 'src/components/MainPageLayout/MapArea.vue';
import TrackingActivation from 'src/components/MainPageLayout/TrackingActivation.vue';
import { useObjectObservedStore } from 'stores/object-observed.js';
import { useConnectionStore } from 'stores/ConnectionStore.js';
import { onMounted, ref, watch } from 'vue';
import { Device } from '@capacitor/device';
import { urlApi } from "src/additional-configuration";


export default ({
  name: 'MainPage',
  components: {
    LogoIcon,
    MapArea,
    TrackingActivation,
    ControlPanel,
    GeoLocation
  },

  setup() {
    const axios = require('axios');
    const connectionStatusStore = useConnectionStore();
    const objectObservedStore = useObjectObservedStore();

    const activation = ref(false);
    const violationStatus = ref([])
    const trackerCoords = ref([])
    const lastUpdateTimestamp = ref('-');
    const deviceId = ref('')
    const zonesData = ref([]);

    watch(objectObservedStore.objectObservedForWatch, (newValue, oldValue) => {
      var violatedNames = []
      if (!!newValue.violation)
        newValue.violation.forEach(v => {
          violatedNames.push(zonesData.value.find(zone => zone.id == v).zoneName)
        })
      violationStatus.value = violatedNames
    })

    onMounted(async () => {
      handleDeviceInfo()
    })

    const handleDeviceInfo = async () => {
      const { uuid: id } = await Device.getId();
      deviceId.value = id;
      zonesData.value = await getAssignedZones(deviceId.value)
      const deviceRequest = {
        id: deviceId.value,
        name: ""
      };
      try {
        const response = await axios.post(`${urlApi}/Device`, deviceRequest);
        connectionStatusStore.status = response.status;
        handleLocalStorage();
      } catch (e) {
        connectionStatusStore.status = e.response.status;
      }
    }
    const getAssignedZones = async (deviceId) => {
      try {
        const getZones = await axios.get(`${urlApi}/zone/spec?AssignedToDeviceId=${deviceId}`)
        return getZones.data.zones;
      } catch (e) {
        e.response
          ? connectionStatusStore.status = e.response.status
          : connectionStatusStore.status = 0;
      }
    }
    const handleLocalStorage = async () => {
      var currentData = JSON.parse(localStorage.getItem('emergencyDataStorage')) || [];
      if (!!currentData) {
        currentData.forEach(async data => {
          const dataResponse = await axios.post(`${urlApi}/observedobject`, data);
          if (dataResponse.status == 200) await currentData.splice(currentData.indexOf(data), 1);
        })
        !currentData ? localStorage.removeItem('emergencyDataStorage') : localStorage.setItem('emergencyDataStorage', JSON.stringify(currentData))
      }
    }

    const activationState = (val) => {
      val
        ? activation.value = true
        : isNotActive()
    }
    const isNotActive = () => {
      activation.value = false;
      violationStatus.value = []
    }

    const positionChanged = async (val) => {
      const gpsDataRequest = {
        deviceId: deviceId.value,
        latitude: val.latitude,
        longitude: val.longitude,
        recordedAt: new Date(),
      }
      lastUpdateTimestamp.value = gpsDataRequest.recordedAt.toLocaleString();
      trackerCoords.value = { latitude: val.latitude, longitude: val.longitude }
      try {
        const gpsDataResponse = await axios.post(`${urlApi}/observedobject`, gpsDataRequest);
        connectionStatusStore.status = gpsDataResponse.status;
      } catch (e) {
        saveToLocalStorage(gpsDataRequest);
        e.response
          ? connectionStatusStore.status = e.response.status
          : connectionStatusStore.status = 0;
      }
    }
    const saveToLocalStorage = (newData) => {
      var currentData = JSON.parse(localStorage.getItem('emergencyDataStorage')) || [];
      currentData.push(newData)
      localStorage.setItem('emergencyDataStorage', JSON.stringify(currentData));
    }

    return {
      violationStatus,
      trackerCoords,
      lastUpdateTimestamp,
      positionChanged,
      activationState,
      activation,
      zonesData,
    }
  }
})
</script>
