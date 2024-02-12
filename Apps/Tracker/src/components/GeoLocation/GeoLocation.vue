<script>
import { ref, watch, computed } from 'vue'
import { Geolocation } from '@capacitor/geolocation'

export default {
  props: {
    activation: {
      type: Boolean,
      required: true
    }
  },
  setup(props, { emit }) {
    const geoId = ref('');
    const activationListener = computed(() => {
      return props.activation ? true : false
    });

    watch(activationListener, (newValue, oldValue) => {
      onActivationChange(newValue);
    })

    const onActivationChange = async (activationState) => {
      if (activationState) {
        geoId.value = await Geolocation.watchPosition({}, (newPosition, err) => {
          if (!!newPosition) {
            emit("positionChanged", { longitude: newPosition.coords.longitude, latitude: newPosition.coords.latitude });
          }
          else {
            console.log("Lack of location data")
          }
        });
      }
      else {
        clearPositionWatch();
      }
    }

    const clearPositionWatch = async () => {
      await Geolocation.clearWatch({ id: geoId.value });
    }
  }
}
</script>
