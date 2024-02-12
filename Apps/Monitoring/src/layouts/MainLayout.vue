<template>
  <q-layout view="lHh Lpr lFf" class="shadow-2 rounded-borders">

    <q-drawer v-model="drawer" show-if-above bordered :mini="miniState" @mouseover="miniState = false"
      @mouseout="miniState = true" mini-to-overlay :breakpoint="500" :class="'bg-grey-9'">
      <q-list padding>
        <q-item dark class="bg-dark">
          <q-item-section avatar>
            <q-icon><img alt="Fish" src="~assets/sledzik.svg"></q-icon>
          </q-item-section>
          <q-item-section>
            <q-item-label>MONITORING</q-item-label>
            <q-item-label caption>powered By Tracker</q-item-label>
          </q-item-section>
        </q-item>
        <q-item dark>
          <q-item-section avatar>
            <q-icon name="account_circle" />
          </q-item-section>
          <q-item-section>
            <q-item-label>GUEST</q-item-label>
            <q-item-label caption>GuestPol Sp. z o.o.</q-item-label>
          </q-item-section>
          <br /><br />
        </q-item>
        <br /><br />
        <EssentialLink v-for="link in essentialLinks" :key="link.title" v-bind="link" @click="refreshHighlight(link)" />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script>
import { defineComponent, ref } from "vue";
import EssentialLink from "components/EssentialLink.vue";
const linksList = [
  {
    title: "Monitoring",
    caption: "Main page",
    icon: "home",
    link: "",
    isActive: true
  },
  {
    title: "Devices",
    caption: "Device manager",
    icon: "smartphone",
    link: "#/devs",
    isActive: false
  },
  {
    title: "Zones",
    caption: "Zone manager",
    icon: "map",
    link: "#/zones",
    isActive: false
  },
  {
    title: "Analysis",
    caption: "History review",
    icon: "public",
    link: "#/analysis",
    isActive: false
  },
  {
    title: "Manual",
    caption: "User guide",
    icon: "school",
    link: "https://github.com/damiandrygala/Sledzik",
    isActive: false
  },
  {
    title: "About",
    caption: "Software description",
    icon: "question_mark",
    link: "https://github.com/damiandrygala/Sledzik",
    isActive: false
  },
];


export default defineComponent({
  name: "MainLayout",

  components: {
    EssentialLink,
  },

  setup() {
    const refreshHighlight = (link) => {
      linksList.forEach((lnk) => {
        lnk.isActive = false;
      })
      link.isActive = true;
    }

    return {
      drawer: ref(false),
      miniState: ref(true),
      essentialLinks: linksList,
      refreshHighlight
    };
  },
});
</script>
