<template>
  <q-page padding class="bg-grey-3">
    <div class="q-pa-md q-gutter-md">
      <h5>Device manager</h5>
      <div class="row justify-between">
        <div class="col-4">
        </div>
        <div class="col-1.5">
          <q-btn color="red-9" icon-right="remove" label="Remove selected" no-caps @click="removeSelectedRecords()" />
        </div>
      </div>
      <br />
      <q-table flat boundered :columns="columns" :rows="rows" :separator="'vertical'" virtual-scroll
        v-model:pagination="pagination" :selected-rows-label="getSelectedRecordsString" selection="multiple"
        v-model:selected="selectedRecords" no-data-label="There is no results" :rows-per-page-options="[0]">
        <!-- NO DATA -->
        <template v-slot:no-data="{ icon, message }">
          <div class="full-width row flex-center text-accent q-gutter-sm">
            <q-icon size="2em" name="sentiment_dissatisfied" />
            <span>
              {{ message }}
            </span>
            <q-icon size="2em" :name=icon />
          </div>
        </template>
        <!-- STATUS COLUMN -->
        <template v-slot:body-cell-status="{ row }">
          <q-td>
            <q-btn flat :label="getStatusBtnLabel(row.username)" :color="getStatusBtnColor(row.username)" icon="task_alt"
              @click="showStatusPrompt(row.id, row.username)" />
            <q-dialog v-model="statusDialog">
              <q-card style="min-width: 350px">
                <q-card-section>
                  <div class="text-h10">Device is already registered.</div>
                  <div class="text-h6">Do you want to modify registration?</div>
                </q-card-section>
                <q-card-section class="q-pt-none">
                  <q-input dense v-model="inputDevName" placeholder="USERNAME" autofocus
                    @keyup.enter="regDevice(statusId, inputDevName)" />
                  <q-card-actions align="right" class="text-primary">
                    <q-btn flat label="CANCEL" v-close-popup color='grey' icon="cancel" />
                    <q-btn flat label="UNASSIGN" v-close-popup color="negative" icon="delete"
                      @click="removeAssignment(statusId)" />
                    <q-btn flat label="RENAME" v-close-popup color="primary" icon="edit"
                      @click="regDevice(statusId, inputDevName)" />
                  </q-card-actions>
                </q-card-section>
              </q-card>
            </q-dialog>
            <q-dialog v-model="uaStatusDialog">
              <q-card style="min-width: 350px">
                <q-card-section>
                  <div class="text-h6">Enter username of device ...</div>
                </q-card-section>
                <q-card-section class="q-pt-none">
                  <q-input dense v-model="inputDevName" autofocus @keyup.enter="regDevice(statusId, inputDevName)"
                    placeholder="USERNAME" />
                </q-card-section>
                <q-card-actions align="right" class="text-primary">
                  <q-btn flat label="Cancel" v-close-popup />
                  <q-btn flat label="Add username" color="positive" icon="save" v-close-popup
                    @click="regDevice(statusId, inputDevName)" />
                </q-card-actions>
              </q-card>
            </q-dialog>
          </q-td>
        </template>

        <!-- ASSIGNED ZONES COLUMN -->
        <template v-slot:body-cell-zones="{ row }">
          <q-td>
            <q-btn :color="getAsZonesBtnColor(row.assignedZones)" :label="getAsZonesBtnLabel(row.assignedZones)"
              icon="map" @click="showAsZonesPrompt(row.id, row.username, row.assignedZones)" />
            <q-dialog v-model="asZonesDialog">
              <q-card style="min-width: 350px">
                <q-card-section>
                  <div class="text-h6">Zone manager of device ...</div>
                </q-card-section>
                <q-card-section class="q-pt-none">
                  <div class="q-gutter-md row">
                    <q-select filled v-model="zoneSelection" use-input use-chips multiple fill-input input-debounce="0"
                      :options="asZonesOptions" @filter="filterFn" hint="Assigned zones"
                      style="width: 250px; padding-bottom: 32px">
                    </q-select>
                  </div>
                </q-card-section>
                <q-card-actions align="right" class="text-primary">
                  <q-btn flat label="Cancel" v-close-popup />
                  <q-btn flat label="CLEAR ALL" color="negative" icon="delete" @click="clearDevAssignment()" />
                  <q-btn flat label="Save" color="positive" icon="save" v-close-popup
                    @click="modifyAsZones(asZonesDevId)" />
                </q-card-actions>
              </q-card>
            </q-dialog>
          </q-td>
        </template>

        <!-- REMOVE COLUMN -->
        <template v-slot:body-cell-remove="{ row }">
          <q-td>
            <q-btn flat color="negative" icon="delete" @click="removeRecord(row.id)" />
          </q-td>
        </template>
      </q-table>
    </div>
  </q-page>
</template>

<script>
import { ref, onMounted } from "vue";
import { urlApi } from "src/additional-configuration";
import { getDevsColumns, getDevsRows } from "src/composables/devs/getDevsData";

export default {
  setup() {
    const axios = require('axios');
    const statusDialog = ref(false);
    const inputDevName = ref('');
    const asZonesDialog = ref(false);
    const asZonesOptions = ref([]);
    const uaStatusDialog = ref(false);
    var getAllZones = [];
    const zoneSelection = ref([]);
    const assignedZonesIds = ref([]);
    const assignedZonesNames = ref([]);
    const statusId = ref('');
    const asZonesDevId = ref('');
    const selectedRecords = ref([]);

    const columns = ref([]);
    const rows = ref([]);

    onMounted(async () => {
      loadData();
    });

    const loadData = async () => {
      try {
        columns.value = await getDevsColumns();
        rows.value = await getDevsRows();
      }
      catch (error) {
        alert("Seeding cells has generated error. Check if server is active");
      }
    }

    const getStatusBtnLabel = (name) => !!name ? name : 'UNASSIGNED';
    const getStatusBtnColor = (name) => !!name ? 'positive' : 'negative';
    const getAsZonesBtnLabel = (assginedZones) => !!assginedZones ? assginedZones.length : '0'
    const getAsZonesBtnColor = (assginedZones) => !!assginedZones ? 'positive' : 'grey'

    const showStatusPrompt = (id, name) => {
      if (!!name) {
        statusDialog.value = true;
        statusId.value = id;
      } else {
        uaStatusDialog.value = true;
        statusId.value = id;
      }
    }
    const regDevice = async (id, name) => {
      if (!!name) {
        const updateDevice = {
          id: id,
          name: name
        }
        await axios.put(`${urlApi}/Device`, updateDevice)
        inputDevName.value = '';
      }
      else console.log("username is empty. Registration failed")
      loadData();
      statusDialog.value = false;
    }
    const removeAssignment = async (id) => {
      const updateDevice = {
        id: id,
        name: ''
      }
      await axios.put(`${urlApi}/Device`, updateDevice);
      loadData();
      statusDialog.value = false
    }

    const showAsZonesPrompt = async (id, name, assignedZones) => {
      if (!!name) {
        assignedZonesIds.value = [];
        assignedZonesNames.value = [];
        zoneSelection.value = [];
        getAllZones = [];
        const getAllZonesRequest = await axios.get(`${urlApi}/Zone`);
        if (!!getAllZonesRequest)
          getAllZonesRequest.data.zones.forEach(zone => {
            getAllZones.push(zone.zoneName);
          })
        if (!!assignedZones)
          assignedZones.forEach(id => {
            const zones = getAllZonesRequest.data.zones.find(zone => zone.id == id);
            assignedZonesIds.value.push(zones.id)
            assignedZonesNames.value.push(zones.zoneName)
          })
        zoneSelection.value = assignedZonesNames.value;
        asZonesDevId.value = id;
        asZonesDialog.value = true;
      }
      else alert("You need to register device first!");
    }
    const clearDevAssignment = () => {
      zoneSelection.value = [];
    }
    const modifyAsZones = async (id) => {
      const selectedZonesIds = ref([]);
      const getAllZonesRequest = await axios.get(`${urlApi}/Zone`);
      zoneSelection.value.forEach(selectedZone => {
        getAllZonesRequest.data.zones.forEach(zone => {
          if (zone.zoneName == selectedZone) selectedZonesIds.value.push(zone.id);
        });
      })
      const updateAsZones = {
        deviceID: id,
        zoneIDs: selectedZonesIds.value
      };
      await axios.post(`${urlApi}/DeviceToZone`, updateAsZones);
      zoneSelection.value = [];
      loadData();
      asZonesDialog.value = false
    }

    const removeRecord = async (id) => {
      await axios.delete(`${urlApi}/Device`, id);
      loadData();
    }

    const removeSelectedRecords = () => {
      selectedRecords.value.forEach(async record => {
        await axios.delete(`${urlApi}/Device`, record.id);
      })
      loadData();
    }


    return {
      //new
      statusId, asZonesDevId,
      // TABLE INITS
      rows, columns, separator: ref('vertical'), getStatusBtnLabel, getStatusBtnColor, getAsZonesBtnLabel, getAsZonesBtnColor,
      // STATUS PROMPT
      statusDialog, uaStatusDialog, inputDevName, showStatusPrompt, regDevice, removeAssignment,
      // AS_ZONES PROMPT
      asZonesDialog, zoneSelection, asZonesOptions, showAsZonesPrompt, clearDevAssignment, modifyAsZones,
      removeRecord, removeSelectedRecords,

      filterFn(val, update, abort) {
        update(() => {
          if (val === '') {
            asZonesOptions.value = getAllZones
          }
          else {
            const needle = val.toLowerCase()
            asZonesOptions.value = getAllZones.filter(v => v.toLowerCase().indexOf(needle) > -1)
          }
        })
      },

      selectedRecords,
      getSelectedRecordsString() {
        return selectedRecords.value.length === 0 ? '' : `${selectedRecords.value.length} record${selectedRecords.value.length > 1 ? 's' : ''} selected of ${rows.value.length}`
      },

      //pagination
      pagination: ref({
        rowsPerPage: 10
      }),
    }
  }
}
</script>

