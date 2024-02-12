<template>
  <div id="map" class="h-full z-[1]"></div>
</template>

<script>
import { onMounted, watch, toRef } from "vue";
import { findCenterPointOnMap } from "src/composables/findCenterPointOnMap"

export default {
  name: "DisplayMap",
  props: {
    zoneCoords: {
      type: Object
    },
    trackerCoords: {
      type: Object
    },
  },
  setup(props, { emit }) {
    let map;
    let centerPoint = [0, 0];
    let marker; let polygon;
    const zonesPolygonGroup = new leaflet.layerGroup();
    const zoneData = toRef(props, 'zoneCoords');

    onMounted(() => {
      centerPoint = findCenterPointOnMap(props.zoneCoords);
      map = leaflet.map("map").setView(centerPoint, 18);
      leaflet
        .tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
          maxZoom: 19,
        })
        .addTo(map);
      if (!!props.zoneCoords) displayZoneCoords(props.zoneCoords);
      if (!!props.trackerCoords) displayTrackerCoords(props.trackerCoords);
    });

    watch(props.trackerCoords, (newValue, oldValue) => {
      displayTrackerCoords(newValue);
    })
    watch(zoneData, (newValue, oldValue) => {
      displayZoneCoords(newValue);
    })

    const displayZoneCoords = (c) => {
      if (!!map.hasLayer(zonesPolygonGroup)) zonesPolygonGroup.clearLayers();
      c.forEach(coords => {
        if (!!coords) {
          const coordinates = [];
          coords.forEach(coordinate => {
            coordinates.push([
              coordinate.latitude,
              coordinate.longitude
            ])
          })
          polygon = new leaflet.polygon(coordinates);
          zonesPolygonGroup.addLayer(polygon);
          map.fitBounds(coordinates);
        }
      })
      zonesPolygonGroup.addTo(map)
    }

    const displayTrackerCoords = (coords) => {
      if (!!coords) {
        if (marker != null) map.removeLayer(marker);
        marker = new leaflet.circleMarker([coords.latitude ?? 0, coords.longitude ?? 0], {
          radius: 7,
          stroke: true,
          color: 'black',
          opacity: 1,
          weight: 1,
          fill: true,
          fillColor: "red",
          fillOpacity: 1
        });
        map.addLayer(marker);
      }
    }

    return {}
  }
}
</script>

<style>
#map {
  height: 600px;
  width: 900px;
}
</style>
