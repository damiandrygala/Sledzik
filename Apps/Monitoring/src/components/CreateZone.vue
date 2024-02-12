<template>
      <q-stepper v-model="step" header-nav ref="stepper" color="primary" animated>
        <q-step :name="1" title="Zone parameters" icon="settings" :done="step > 1" @keyup.enter="() => { step = 2 }">
          Enter name of a new zone
          <q-input filled v-model="zoneName" label="ZONE NAME *" lazy-rules
            :rules="[val => val && val.length > 0 || 'Please input Zone name']" />
          <q-stepper-navigation>
            <q-btn @click="() => { step = 2 }" color="primary" label="Continue" />
          </q-stepper-navigation>
        </q-step>

        <q-step :name="2" title="Zone definition" icon="create_new_folder" :done="step > 2" @keyup.enter="onSubmit">
          Define a Zone
          <DefinePolygon @coordsDrawn="coordsDrawn" :operation="'draw'" />
          <q-stepper-navigation>
            <q-btn flat @click="step = 1" color="primary" label="Back" class="q-ml-sm" />
            <q-btn color="primary" @click="onSubmit" label="Finish" />
          </q-stepper-navigation>
        </q-step>
      </q-stepper>
</template>

<script>
import DefinePolygon from './MapOperations.vue';
import { ref } from 'vue'
import { urlApi } from "src/additional-configuration";

export default {
  components: { DefinePolygon },
  setup(props, { emit }) {
    const axios = require("axios");
    const zoneName = ref(null);
    const request = {
      zone: {
        ZoneName: "",
        coordinates: [],
      }
    }

    const coordsDrawn = (val) => {
      request.zone.coordinates = val.coordinates;
    };

    const onSubmit = async () => {
      request.zone.ZoneName = zoneName.value;
      if (!!request.zone.ZoneName && !!request.zone.coordinates.length) {
        await axios.post(`${urlApi}/zone`, request);
        emit('onCreated')
      }
      else alert('Nazwa oraz definicja strefy muszą być określone!');
    };

    return {
      coordsDrawn,
      zoneName,
      step: ref(1),
      onSubmit,
    }
  }
}
</script>
