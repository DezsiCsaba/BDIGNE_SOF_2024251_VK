<template>
<!--  DEBUG-->
<!--  <div>{{ ticket }}</div>-->

  <q-card
    style="max-height: 100px; max-width: 400px"
    class="q-my-md"
  >
    <q-card-section :horizontal="true" class="flex flex-center q-pa-sm">
      <div class="text-bold">{{ ticket.title }}</div>

      <q-separator class="q-mx-sm" :vertical="true"/>

      <div class="wrap text-caption">{{ ticket.description }}</div>

    </q-card-section>
    <div :class="bgColorFn()" class="full-width" style="height: 5px"></div>
  </q-card>

</template>

<script setup>
defineOptions({
  name: 'TicketOverwiev'
})

const colorMap = {
  Low: 'bg-primary',
  Medium: 'bg-warning',
  High: 'bg-negative text-bold text-uppercase'
}
const bgColorFn = () => {
  console.log('bg-color', colorMap[props.ticket.priority])
  return colorMap[props.ticket.priority]
}

const props = defineProps({
  ticket: {type: Object}
})

</script>
