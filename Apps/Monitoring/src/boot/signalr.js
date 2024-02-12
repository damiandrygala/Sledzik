import { boot } from "quasar/wrappers";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { useObjectObservedStore } from "stores/object-observed";
import { urlApi } from "src/additional-configuration";

// "async" is optional;
// more info on params: https://v2.quasar.dev/quasar-cli/boot-files
export default boot(async ({ app, store }) => {
  const observedObjectStore = useObjectObservedStore(store);
  initializeSignalR(observedObjectStore);
});

async function initializeSignalR(store) {
  console.debug("[SignalR] InitializeSignalR. Trying to connect...");
  let connection = new HubConnectionBuilder()
    .withUrl(`${urlApi}/observedObjectCoordinates`)
    .build();

  connection.on("CoordinatesObservedObjectReceived", (data) => {
    store.objectObserved.deviceId = data.deviceId;
    store.objectObserved.latitude = data.coordinates.latitude;
    store.objectObserved.longitude = data.coordinates.longitude;
    store.objectObserved.recordedAt = data.recordedAt;
    store.objectObserved.violation = data.violatedZoneIds;
  });

  await connection.start().then(function () {
    console.log("[SignalR] connected");
  });
  return connection;
}
