<template>
  <q-card-section :horizontal="false" class="q-pa-xs q-ma-md full-height" style="width: max-content">
    <div class="text-h6 q-mb-md">Tickets</div>
<!--    TODO - dinamikus scroll bar-->
    <div
      v-for="t in list" :key="t"
      class="q-my-xs clickable"
      @click="setCurrentTicket(t)"
    >
      <q-card-section :horizontal="true" class="flex justify-start items-center">
        <q-icon name="upload" class="q-mx-sm"/>
        <q-card-section class="q-py-sm q-px-none">
          <a target="_blank" :href="getTicketUrl(t)">{{ getShortTicketTitle(t) }}</a>
          <div class="no-wrap" style="white-space: nowrap; overflow: hidden">{{ cutAfterCharIndex(t.description, 20) }}</div>
        </q-card-section>
      </q-card-section>
      <q-separator/>
    </div>
  </q-card-section>

</template>

<script setup>
import {onMounted, ref} from "vue";
import { useTicketStore } from "stores/ticket";
import _ from "lodash";

const props = defineProps({
  project: {type: Object}
})
const emits = defineEmits(['loaded', 'selected'])
const ticketStore = useTicketStore()

const list = ref([])
const loaded = ref(false)

const load = async () => {
  await ticketStore.getTicketsByProjectId({projectId: props.project.id})
  const tickets = _.cloneDeep(ticketStore.list) || []

  list.value = _.sortBy(tickets, 'createdAt').reverse()
  setCurrentTicket(list.value[0])
  loaded.value = true
  emits('loaded')
}

const setCurrentTicket = (ticket) => {
  ticketStore.currentTicket = ticket
  emits('selected')
}

const cutAfterCharIndex = (value, index) => {
  return value.slice(0, index) + '...'
}
const getShortTicketTitle = (ticket) => {
  return `${props.project.shortName}-${ticket.id}`
}

const getTicketUrl = (ticket) => {
  return window.location.origin + '/#/ticket/' + ticket.id
}

onMounted(async () => {
  await load()
})
</script>
