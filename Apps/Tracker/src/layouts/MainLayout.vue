<template>
  <router-view />
</template>

<script>
import { defineComponent } from 'vue'
import { Dark } from 'quasar'
export default defineComponent({
  name: 'MainLayout',
  setup() {
    Dark.set(true);
  }
})
</script>
