import { urlApi } from "src/additional-configuration";
import { ref } from "vue";

export async function getZonesColumns() {
  const columns = ref([]);
  columns.value = [
    {
      name: "zoneId",
      required: true,
      label: "Zone ID",
      align: "center",
      field: (row) => row.id,
      format: (val) => `${val}`,
      sortable: true,
    },
    {
      name: "zoneName",
      required: true,
      label: "Zone Name",
      align: "center",
      field: (row) => row.zoneName,
      format: (val) => `${val}`,
      sortable: true,
    },
    {
      name: "display",
      required: true,
      label: "Display on map",
      align: "center",
      field: "display",
      sortable: true,
    },
    {
      name: "remove",
      required: true,
      label: "Remove",
      align: "center",
      field: "remove",
    },
  ];
return columns.value;
}

export async function getZonesRows() {
  const axios = require("axios");
  const zonesDetailsMap = ref([]);
  const getAllZones = await axios.get(`${urlApi}/zone`);
  getAllZones.data.zones.forEach((zone) => {
    zonesDetailsMap.value.push({
      id: zone.id,
      zoneName: zone.zoneName,
      coordinates: zone.coordinates,
    });
  });
  return zonesDetailsMap.value;
}
