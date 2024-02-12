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
    operation: {
      type: String, // 'display' or 'draw' only
      required: true
    }
  },
  setup(props, { emit }) {
    let map;
    let centerPoint = [0, 0];
    let marker;
    const trackerCoords = toRef(props, 'trackerCoords');

    onMounted(() => {
      if (props.operation == 'display') centerPoint = findCenterPointOnMap(props.zoneCoords);
      if (props.operation == 'draw') centerPoint = [51.0783027, 17.0619564];
      map = leaflet.map("map").setView(centerPoint, 18);
      leaflet
        .tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
          maxZoom: 19,
          attribution:
            '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        })
        .addTo(map);
      mapOperationsHandler(props.operation)
    });

    const mapOperationsHandler = (operation) => {
      if (operation == 'display') {
        displayZoneCoords(props.zoneCoords)
        if (!!props.trackerCoords) displayTrackerCoords(props.trackerCoords)
      }
      else if (operation == 'draw') {
        drawZoneCoords()
      }
      else console.log('invalid map operation')
    }

    const drawZoneCoords = () => {
      let listener = [];
      let coordinates = [];
      map.pm.addControls({
        position: "topleft", drawMarker: false, drawCircleMarker: false, drawPolyline: false, drawRectangle: false, drawCircle: false, drawText: false, editMode: false, dragMode: false, cutPolygon: false, removalMode: false, rotateMode: false
      });
      map.on("pm:drawstart", ({ workingLayer }) => {
        workingLayer.on("pm:vertexadded", (e) => {
          listener.push(e.latlng);
        });
      });
      // When drawing has been finished
      map.on("pm:drawend", (e) => {
        listener.forEach((coordListener) => {
          coordinates.push({
            latitude: coordListener.lat,
            longitude: coordListener.lng
          })
        })
        emit("coordsDrawn", { coordinates });
      });
      var drawnItems = new leaflet.FeatureGroup();
      map.addLayer(drawnItems);
    }

    watch(trackerCoords, (newValue, oldValue) => {
      displayTrackerCoords(newValue);
    })

    const displayZoneCoords = (coords) => {
      if (!!coords) {
        const coordinates = [];
        coords.forEach(coordinate => {
          coordinates.push([
            coordinate.latitude,
            coordinate.longitude
          ])
        })
        var polygon = new leaflet.polygon(coordinates);
        map.addLayer(polygon);
        map.fitBounds(coordinates);
      }
    }

    const displayTrackerCoords = (coords) => {
      if (!!coords) {
        if (marker != null) map.removeLayer(marker);
        marker = new leaflet.circleMarker([coords.latitude, coords.longitude], {
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

