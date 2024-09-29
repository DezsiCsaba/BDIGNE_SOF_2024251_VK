<template>
<!--  <template v-if="val === true">-->
<!--    <span :class="`material-icons iconField ${icons[type].positiveColor}`">{{ icons[type].positive }}</span>-->
<!--  </template>-->
<!--  <template v-else>-->
<!--    <span :class="`material-icons iconField ${icons[type].negativeColor}`">{{ icons[type].negative }}</span>-->
<!--  </template>-->
  <q-icon :class="`material-icons`" size="xs">
    {{ typeOfIcon }}
    <q-tooltip v-if="tooltip" anchor="top middle" :offset="[25, 25]">
      {{ tooltip }}
    </q-tooltip>
  </q-icon>

</template>

<script setup>
import {ref} from "vue";

const props = defineProps(['type', 'val', 'tooltip'])

// const icons = {
//   bell: { positive: "notifications", negative: "notifications_off", positiveColor: 'text-positive', negativeColor: 'text-negative' },
// }

const value = ref(!!props?.val)
const typeOfIcon = ref(props?.type ? props.type : 'primary') //megjelenítendő icon
</script>
