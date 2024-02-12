<template>
  <br /><br />
  <div class=q-pa-lg>
    <div class="flex flex-center">
      <img alt="Fish" src="~assets/sledzik.svg" style="width: 200px; height: 200px">
      <br /><br />
      Connection status: {{ connectionStatusStore.status }}
      <br />
      Description: {{ getDescription() }}
    </div>
  </div>
</template>

<script>
import { defineComponent, onMounted, ref } from 'vue'
import { useConnectionStore } from 'stores/ConnectionStore.js'
export default defineComponent({
  name: 'DebugPage',
  components: {
  },

  setup() {
    const connectionStatusStore = useConnectionStore();
    const getDescription = () => {
      switch (connectionStatusStore.status) {
        case 0:
          return "Check your internet connection";
        case 404:
          return "Not found";
        case 500:
          return "Error 500: internal server - zweryfikuj czy obiekt został zarejestrowany";
        default:
          return "Unexpected error. Search in google";
      }
    }
    return {
      connectionStatusStore,
      getDescription
    }
  }

})
</script>

<style></style>
