<template>
  <q-page padding class="bg-grey-3">
    <div class="h-screen relative">
      <h5>Zone manager</h5>
      <div class="row justify-between">
        <div class="col-4">
          <q-btn color="primary" icon-right="add" label="Create" no-caps @click="createZone()" />
        </div>
        <div class="col-1.5">
          <q-btn color="red-9" icon-right="remove" label="Remove selected" no-caps @click="removeSelectedRecords()" />
        </div>
      </div>
      <br />
      <q-table flat boundered :columns="columns" :rows="rows" :separator="'vertical'" virtual-scroll
        v-model:pagination="pagination" :selected-rows-label="getSelectedRecordsString" selection="multiple"
        v-model:selected="selectedRecords" no-data-label="There is no results" :rows-per-page-options="[0]">
        <!-- Display Zone -->
        <template v-slot:body-cell-display="{ row }">
          <q-td><q-btn flat icon="map" @click="displayOnMap(row.id)" /></q-td>
        </template>
        <!-- Remove zone -->
        <template v-slot:body-cell-remove="{ row }">
          <q-td>
            <q-btn flat color="negative" icon="delete" @click="removeRecord(row.id)" />
          </q-td>
        </template>
        <!-- NO DATA -->
        <template v-slot:no-data="{ icon, message }">
          <div class="full-width row flex-center text-accent q-gutter-sm">
            <q-icon size="2em" name="sentiment_dissatisfied" />
            <span>
              {{ message }}
            </span>
            <q-icon size="2em" :name="icon" />
          </div>
        </template>
      </q-table>

      <q-dialog v-model="displayMapFlag" full-width>
        <DisplayMap :zoneCoords="zoneCoords" :operation="'display'" />
      </q-dialog>
      <q-dialog v-model="createZoneFlag" full-width>
        <CreateZone @onCreated="onCreated" />
      </q-dialog>
    </div>
  </q-page>
</template>

<script>
import { ref, defineComponent, onMounted } from 'vue';
import DisplayMap from 'src/components/MapOperations.vue';
import CreateZone from 'src/components/CreateZone.vue';
import { urlApi } from "src/additional-configuration";
import { getZonesColumns, getZonesRows } from 'src/composables/zones/getZonesData';

export default defineComponent({
  components: {
    DisplayMap,
    CreateZone,
  },
  setup(props, { emit }) {
    const axios = require("axios");
    const columns = ref([]);
    const rows = ref([]);
    const createZoneFlag = ref(false);
    const displayMapFlag = ref(false);
    const zoneCoords = ref(null);
    const selectedRecords = ref([]);

    onMounted(() => {
      loadData();
    })

    const loadData = async () => {
      console.log("Seeding has been started");
      try {
        columns.value = await getZonesColumns();
        rows.value = await getZonesRows();
      }
      catch (error) {
        alert("Seeding cells has generated error. Check if server is active");
      }
    }

    const createZone = () => {
      createZoneFlag.value = true;
    }

    const onCreated = () => {
      createZoneFlag.value = false;
      loadData();
    }

    const displayOnMap = (id) => {
      zoneCoords.value = rows.value.find(r => r.id == id).coordinates;
      displayMapFlag.value = true;
    };

    const removeRecord = async (id) => {
      await axios.delete(`${urlApi}/Zone`, rows.value.find(r => r.id == id).id);
      loadData();
    };

    const removeSelectedRecords = () => {
      selectedRecords.value.forEach(async record => {
        await axios.delete(`${urlApi}/Zone`, record.id);
      })
      loadData();
    }


    return {
      columns, rows, createZoneFlag, displayMapFlag, zoneCoords,
      createZone, displayOnMap, removeRecord, removeSelectedRecords, onCreated,
      pagination: ref({
        rowsPerPage: 10
      }),

      selectedRecords,
      getSelectedRecordsString() {
        return selectedRecords.value.length === 0 ? '' : `${selectedRecords.value.length} record${selectedRecords.value.length > 1 ? 's' : ''} selected of ${rows.value.length}`
      },
    }
  }
})
</script>
