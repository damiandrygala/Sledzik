import { urlApi } from "src/additional-configuration";
import { ref } from "vue";

export async function getDevsColumns() {
  const columns = ref([]);
  columns.value = [
    {
      name: 'id',
      required: true,
      label: 'Device ID',
      align: 'left',
      field: row => row.id,
      format: val => `${val}`,
      sortable: true
    },
    { name: 'status', align: 'center', label: 'Status', field: 'status', sortable: true },
    { name: 'zones', align: 'center', label: 'Assigned zones', field: 'zones', sortable: true },
    { name: 'remove', align: 'center', label: 'Remove', field: 'remove', sortable: true },
  ]
  return columns.value;
}

export async function getDevsRows() {
  const axios = require("axios");
  const rows = ref([]);
  const regDevsRequest = await axios.get(`${urlApi}/Device?NamedOnly=true`);
  const naDevsRequest = await axios.get(`${urlApi}/Device?NamedOnly=false`);
  regDevsRequest.data.deviceItems.forEach(async dev => {
    if (!!dev.id) {
      const assginedZonesAll = await axios.get(`${urlApi}/Zone/spec?AssignedToDeviceId=${dev.id}`);
      const assginedZonesIds = assginedZonesAll.data.zones.map(z => z.id);
      rows.value.push({ id: dev.id, username: dev.name, assignedZones: assginedZonesIds });
    }
  });
  naDevsRequest.data.deviceItems.forEach(dev => {
    if (!!dev.id) rows.value.push({ id: dev.id, username: dev.name});
  });
  return rows.value;
}
