<template>
  <q-card class="my-card" flat>
    <h6 v-if="isAssignee" class="q-ma-xs">Assigned to me</h6>
    <h6 v-if="isAssignee === false" class="q-ma-xs">Reported by me</h6>
    <q-card-section>
      <div>
        <AsyncTable
          v-if="loaded"
          :columns="columns"
          :rows="list"
          :pagination="5"
          :noDataLabel="isAssignee ? 'No ticket assigned to you' : 'No tickets reported by you'"
        ></AsyncTable>
      </div>
    </q-card-section>
  </q-card>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from "vue";
import _ from "lodash";
import { useTicketStore } from "stores/ticket";
import { getColumns } from "src/utils/tableSetup";
import { ticketStatusAndPriorityColorfn } from "src/utils/functions";
import {LocalStore} from "../../utils/helpers";

const props = defineProps({
  isAssignee: { type: Boolean, default: true }
})

const ticketStore = useTicketStore()

const list = ref([])
const loaded = ref(false)

const load = async () => {
  props.isAssignee
    ? list.value = _.sortBy(_.cloneDeep(ticketStore.list).filter(ticket => ticket.assigneeId === LocalStore.getUserData().id), 'updatedAt').reverse() || []
    : list.value = _.sortBy(_.cloneDeep(ticketStore.list).filter(ticket => ticket.reporterId === LocalStore.getUserData().id), 'updatedAt').reverse() || []
  console.log(list.value)
  loaded.value = true
}

const baseMap = {
  title: { tableSetup: { link: '/ticket' } },
  description: { tableSetup: {classes: 'white-space-normal text-left'} },
  status: { tableSetup: { colorFn: ticketStatusAndPriorityColorfn } },
  priority: { tableSetup: { colorFn: ticketStatusAndPriorityColorfn , iconField: true} }
}
const columns = [ ...getColumns(baseMap) ]

onMounted(async ()=>{
  await load()
})

// const TicketOverwiev = defineAsyncComponent(() => import('./Ticket.vue'))
const AsyncTable = defineAsyncComponent(() => import('../customBaseComponents/Table.vue'))


</script>
