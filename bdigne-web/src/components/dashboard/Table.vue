<template>
  <q-card class="my-card" flat>
    <h6 class="q-ma-xs">All tickets</h6>
    <q-card-section>
      <div v-if="list === []">There are no tickets</div>
      <div v-else>
<!--        <li v-for="ticket in list" :key="ticket.id">-->
<!--          {{ ticket.title }}-->
<!--        </li>-->
        <AsyncTable
          v-if="loaded"
          :columns="columns"
          :rows="list"
        ></AsyncTable>
      </div>
    </q-card-section>
  </q-card>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from "vue";
import _ from "lodash";
import { useTicketStore } from "stores/ticket";
import { getColumns, tableSetup } from "src/utils/tableSetup"
import { ticketStatusAndPriorityColorfn } from "src/utils/functions"

defineOptions({
  name: 'TicketTable'
})

const ticketStore = useTicketStore()

const list = ref([])
const loaded = ref(false)

const load = async () => {
  // await ticketStore.load()
  const tickets = _.cloneDeep(ticketStore.list) || []

  list.value = _.sortBy(tickets, 'createdAt').reverse()

  loaded.value = true
}

// const baseMap = {
//   title: {tableSetup: { link: '/ticket' }},
//   description: {tableSetup: {classes: 'white-space-normal text-left'} },
//   status: {tableSetup: { colorFn: ticketStatusAndPriorityColorfn }},
//   priority: {tableSetup: { colorFn: ticketStatusAndPriorityColorfn, iconField: true }},
// }
const baseMap = {
  title: tableSetup ({ link: '/ticket' }),
  description: tableSetup ({classes: 'white-space-normal text-left'}),
  status: tableSetup({ colorFn: ticketStatusAndPriorityColorfn }),
  priority: tableSetup({ colorFn: ticketStatusAndPriorityColorfn, iconField: true }),
}
const dataConfMap = ref(baseMap)
const columns = [
  ...getColumns(baseMap)
]



onMounted(async () => {
  await load()
})

const AsyncTable = defineAsyncComponent(() => import('../customBaseComponents/Table.vue'))

</script>
