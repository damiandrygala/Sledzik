<template>
  <q-page padding class="bg-grey-3">
    <div class="h-screen relative">
      <h4>Welcome to Analysis page</h4>
      <h5> Choose the zone you want previous movements to display </h5>
      <div class="row justify-between">
        <div class="col-4">
        </div>
        <div class="col-1.5">
          <q-btn color="red-9" icon-right="remove" label="Remove selected" no-caps @click="removeSelectedRecords()" />
        </div>
      </div>
      <br />
      <q-table flat boundered :columns="columns" :rows="rows" :separator="'vertical'" virtual-scroll
        v-model:pagination="pagination" :selected-rows-label="getSelectedString" selection="multiple"
        v-model:selected="selected" no-data-label="There is no results" :rows-per-page-options="[0]">
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
        <template v-slot:body-cell-display="{ row }">
          <q-td><q-btn flat icon="map" @click="displayOnMap(row.id)" /></q-td>
        </template>
        <!-- REMOVE COLUMN -->
        <template v-slot:body-cell-remove="{ row }">
          <q-td>
            <q-btn flat color="negative" icon="delete" @click="removeRecord(row.id)" />
          </q-td>
        </template>
      </q-table>
      <div class="q-mt-md">
        Selected: {{ JSON.stringify(selected) }}
      </div>
      <div class="q-pa-md q-gutter-sm">
        <q-dialog v-model="displayMapFlag" full-width>
          <q-card>
            <Timeline :Timeline="timeline" @userChoice="userChoice" />
            <AnalMap :zoneCoords="transferToMap.violatedZoneCoords" :trackerCoords="trackerCoords"
              :operation="'display'" />
          </q-card>
        </q-dialog>
      </div>
    </div>
  </q-page>
</template>

<script>
import { onMounted, ref } from "vue";
import AnalMap from "src/components/MapOperations.vue";
import Timeline from "src/components/TimelineAnal.vue";
import { getAnalColumns, getAnalRows } from "src/composables/analysis/getAnalData";
import { urlApi } from "src/additional-configuration";

export default ({
  name: "AnalysisPage",
  components: { AnalMap, Timeline },
  setup() {
    const axios = require("axios");
    const columns = ref([]);
    const rows = ref([]);
    const displayMapFlag = ref(false);
    const transferToMap = ref(null);
    const timeline = ref([]);


    onMounted(() => {
      loadData();
    })

    const loadData = async () => {
      console.log('Seeding has been started');
      try {
        columns.value = await getAnalColumns();
        rows.value = await getAnalRows();
      }
      catch (error) {
        alert('Seeding cells has generated error. Check if server is active');
      }
    }

    const displayOnMap = (id) => {
      transferToMap.value = rows.value.find(r => r.id == id)
      timeline.value = getTimelineData(transferToMap.value);
      displayMapFlag.value = true;
    }

    const getTimelineData = (val) => {
      let timestamps = []
      val.recordedPositionMap.forEach(v => {
        timestamps.push(v.key)
      })
      return timestamps;
    }

    const trackerCoords = ref(null);
    const userChoice = (val) => {
      const position = transferToMap.value.recordedPositionMap.find(pos => pos.key == val.userChoice.value);
      trackerCoords.value = position.value;
    }

    const removeRecord = async (id) => {
      const uid = rows.value.find(r => r.id == id).uniqueId;
      await axios.delete(`${urlApi}/Analysis`, uid);
      loadData();
    }

    const removeSelectedRecords = () => {
      selected.value.forEach(async record => {
        const uid = rows.value.find(r => r.uniqueId == record.uniqueId).uniqueId;
        await axios.delete(`${urlApi}/Analysis`, uid);
      })
      loadData();
    }

    const selected = ref([]);

    return {
      columns, rows, displayOnMap, displayMapFlag,
      transferToMap, timeline, userChoice, trackerCoords, removeRecord, removeSelectedRecords,
      pagination: ref({
        rowsPerPage: 10
      }),
      selected,
      getSelectedString() {
        return selected.value.length === 0 ? '' : `${selected.value.length} record${selected.value.length > 1 ? 's' : ''} selected of ${rows.value.length}`
      },

    }
  },
});

</script>
