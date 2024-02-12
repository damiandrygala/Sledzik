import { defineStore } from 'pinia';

export const useObjectObservedStore = defineStore('objectObserved', {
  state: () => ({
    objectObserved: {
      deviceId: "",
      latitude: 0,
      longitude: 0,
      recordedAt: "",
      violation: ""
    }
  }),
  getters: {
    objectObservedForWatch(state) {
      return state.objectObserved;
    }
  },
  actions: {
  },
});
