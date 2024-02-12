import { urlApi } from "src/additional-configuration";
import { ref } from "vue";

export async function getAnalColumns() {
  const columns = ref([]);
  columns.value = [
    {
      name: "id",
      required: true,
      label: "Violation ID",
      align: "center",
      field: row => row.id,
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "deviceName",
      required: true,
      label: "Device Name",
      align: "center",
      field: row => row.deviceName,
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "BOV",
      required: true,
      label: "Begin Of Violation",
      align: "center",
      field: row => row.BOV,
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "EOV",
      required: true,
      label: "End Of Violation",
      align: "center",
      field: row => row.EOV,
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "violatedZoneName",
      required: true,
      label: "Violated Zone",
      align: "center",
      field: row => row.violatedZoneName,
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "display",
      required: true,
      label: "Display on map",
      align: "center",
      field: "display",
      format: val => `${val}`,
      sortable: true
    },
    {
      name: "remove",
      label: "Remove",
      align: "center",
      field: "remove",
      format: val => `${val}`,
    },
  ];
    return columns.value;
}

export async function getAnalRows() {
  const axios = require("axios");
  const validateDate = (date) => {
    if (date == '0001-01-01 00:00') return "IN PROGRESS";
    else return date
  }
  const violationDetailsMap = ref([]);
  const response = await axios.get(`${urlApi}/Analysis`);
  const deviceName = await axios.get(`${urlApi}/Device?NamedOnly=true`);
  const allCoords = await axios.get(`${urlApi}/Zone`);
  response.violationAnalysis.forEach(async violation => {
    const coords = allCoords.zone.find(z => z.id == violation.violatedZone).coordinates
    violationDetailsMap.value.push({
      uniqueId: violation.id,
      id: `${violation.deviceId.substring(0, 3)}-${violation.violatedZone.substring(0, 3)}-${new Date(violation.startViolationTs).getMilliseconds()}`,
      deviceId: violation.deviceId,
      deviceName: deviceName.getDevices.find(d => d.deviceId == violation.deviceId).name, //in future - get with param before Map definition
      violatedZoneId: violation.violatedZone,
      violatedZoneName: allCoords.zone.find(z => z.id == violation.violatedZone).zoneName,
      BOV: violation.startViolationTs.slice(0, 16).replace("T", " "),
      EOV: validateDate(violation.endViolationTs.slice(0, 16).replace("T", " ")),
      violatedZoneCoords: coords,
      recordedPositionMap: violation.recordedPositionMap
    })
  })
  return violationDetailsMap.value;
}
