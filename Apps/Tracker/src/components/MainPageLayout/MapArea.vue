<template>
  <div id="map" class="h-full z-[1]"></div>
</template>

<script>
import { onMounted, computed, watch } from 'vue';

export default {
  name: 'MapArea',
  props: {
    trackerCoords: {
      type: Object
    },
    zones: {
      type: Object
    }
  },
  setup(props) {
    var map;
    var defaultPoint = [52.230678, 21.006699]; // Warsaw coords
    var marker; var polygon;
    const zonesPolygonGroup = new leaflet.layerGroup();
    const trackerCoordsRedefined = computed(() => {
      return props.trackerCoords ? props.trackerCoords : null
    })
    const zonesRedefined = computed(() => {
      return props.zones ? props.zones : null
    })

    onMounted(() => {
      map = props.trackerCoords.length != 0
        ? leaflet.map("map").setView([props.trackerCoords.latitude, props.trackerCoords.longitude], 18)
        : leaflet.map("map").setView(defaultPoint, 18)
      leaflet
        .tileLayer("https://tile.openstreetmap.org/{z}/{x}/{y}.png", {
          maxZoom: 19,
          attribution:
            '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>',
        })
        .addTo(map);
    })

    watch(trackerCoordsRedefined, (newValue, oldValue) => {
      displayTrackerCoords(newValue);
    })

    watch(zonesRedefined, (newValue, oldValue) => {
      drawZones(newValue);
    })

    const drawZones = (zones) => {
      if (!zones) return
      if (!!map.hasLayer(zonesPolygonGroup)) zonesPolygonGroup.clearLayers();
      zones.forEach(zone => {
        const coordinates = [];
        zone.coordinates.forEach(coords => {
          if (!!coords) {
            coordinates.push([
              coords.latitude,
              coords.longitude
            ])
          }
        })
        polygon = new leaflet.polygon(coordinates);
        polygon.bindPopup(zone.zoneName)
        zonesPolygonGroup.addLayer(polygon);
      })
      zonesPolygonGroup.addTo(map);
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
        map.setView(marker._latlng, 18)
      }
    }
  },
}
</script>

<style>
#map {
  height: 250px;
  width: 350px;
}
</style>
