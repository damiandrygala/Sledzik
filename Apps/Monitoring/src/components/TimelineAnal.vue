<template>
  <q-slider class="q-mt-l" v-model="TimelineModel" color="deep-orange" markers :marker-labels="getMarkerLabel"
    :min="getMinTimeline" :max="getMaxTimeline" :step="getStepTimeline" @update:model-value="userUpdate" />
</template>

<script>
import { onMounted, ref } from 'vue';

export default ({
  props: {
    Timeline: {
      type: Array,
      required: true
    }
  },
  setup(props, { emit }) {
    const TimelineModel = ref(0);
    const getMinTimeline = ref(0);
    const getMaxTimeline = ref(0);
    const getStepTimeline = ref(1);
    const getMarkerLabel = ref("");

    const userChoice = ref(null);

    onMounted(() => {
      getMinTimeline.value = 0;
      getMaxTimeline.value = props.Timeline.length - 1;
      getMarkerLabel.value = val => props.Timeline[`${val}`].slice(0, 19).replace("T", " ");
      userUpdate();
    })

    const userUpdate = () => {
      userChoice.value = props.Timeline[TimelineModel.value];
      emit('userChoice', { userChoice });
    }

    return ({
      TimelineModel, getMinTimeline, getMaxTimeline, getStepTimeline, getMarkerLabel,
      userUpdate
    });
  }
})
</script>
