import { defineStore } from "pinia";

export const useConnectionStore = defineStore("connection", {
  state: () => ({
    status: 0,
  }),
});
