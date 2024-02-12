<template>
  <q-header elevated>
    <q-toolbar>
      <q-toolbar-title>
        <q-btn round flat>
          <q-avatar size="40px">
            <img src="https://cdn3.iconfinder.com/data/icons/social-messaging-productivity-6/128/profile-circle2-512.png">
          </q-avatar>
        </q-btn>
        GUEST
      </q-toolbar-title>
      <div class="q-gutter-sm row items-center no-wrap">
        <q-btn-dropdown size="20px" round dense flat :color="getViolationColor(violatedZones)" icon="flag">
          <div v-if="violatedZones.length != 0">
            <q-list>
              <q-item v-for="zone in violatedZones" :key="zone.id">{{ zone }}</q-item>
            </q-list>
          </div>
        </q-btn-dropdown>
        <q-btn size="20px" round dense flat color="grey-8" icon="wifi" @click="getConnectionStatus()">
          <q-badge v-if="connectionStatusStore.status != 200" rounded color="red" floating />
        </q-btn>
      </div>
      TRACKER V1.0
    </q-toolbar>
    <q-banner v-if="violatedZones.length != 0" inline-actions class="text-white bg-red">
      <q-icon name="my_location" color="grey-8" size="28px" /> Violation in progress. Click red flag to see details.
    </q-banner>
    <q-banner v-if="connectionStatusStore.status != 200" inline-actions class="text-white bg-orange">
      <q-icon name="signal_wifi_off" color="grey-8" size="28px" /> You have lost connection to the server. Your position
      will be buffered until connection will return
      <template v-slot:action>
        <q-btn flat color="white" icon="handyman" label="DEBUG" @click="getConnectionStatus()" />
      </template>
    </q-banner>
  </q-header>
  <div v-if="violatedZones.length == 0 && connectionStatusStore.status == 200"><br /><br /><br /></div>
  <div v-else><br /></div>
</template>
<script>
import { useRouter } from 'vue-router';
import { useConnectionStore } from 'stores/ConnectionStore.js'

export default {
  name: 'ControlPanel',
  props: {
    violatedZones: {
      type: Array
    }
  },
  setup(props) {
    const connectionStatusStore = useConnectionStore();
    const route = useRouter();

    const getConnectionStatus = () => {
      if (connectionStatusStore.status != 200) route.push('/debug');
    }
    const getViolationColor = (status) => {
      return status.length == 0 ? 'positive' : 'negative'
    }

    return {
      connectionStatusStore,
      getConnectionStatus,
      getViolationColor
    }
  }
}
</script>
