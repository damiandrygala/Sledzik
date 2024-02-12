<template>
  <div class="q-pa-md">
    <div class="column justify-center">
      <q-btn class="full-width" :color="getLaunchDetails().color" :label="getLaunchDetails().label"
        :icon="getLaunchDetails().icon" push size="lg" @click="onLaunchButton()" />
    </div>
  </div>
</template>
<script>
import { ref } from 'vue';
import { useQuasar } from 'quasar'

export default {
  name: 'TrackingActivation',
  setup(props, { emit }) {
    const $q = useQuasar();
    var timer;
    const isClicked = ref(false);
    const launchDetails = {
      isNotClicked: {
        color: 'blue-6',
        label: 'Begin tracking',
        icon: 'timeline'
      },
      isClicked: {
        color: 'red-6',
        label: 'Finish tracking',
        icon: 'map'
      }
    };

    const getLaunchDetails = () => { return !isClicked.value ? launchDetails.isNotClicked : launchDetails.isClicked; }
    const onLaunchButton = () => {
      isClicked.value = !isClicked.value;
      emit("buttonStatus", isClicked.value ? true : false);
      $q.loading.show();
      timer = setTimeout(() => {
        $q.loading.hide();
        timer = void 0
      }, 300)
    }
    return {
      onLaunchButton,
      getLaunchDetails
    }
  }
}
</script>
