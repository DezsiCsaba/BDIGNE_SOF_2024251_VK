<template>
  <q-page class="flex justify-center">
    <AsyncTicket
      v-if="loaded"
      :ticket="ticket"
      @reloadNeeded="load"
    />
  </q-page>
</template>

<script setup>
defineOptions({
  name: 'TicketObserver'
})

import {defineAsyncComponent, onMounted, ref} from "vue";
import { useTicketStore } from "stores/ticket";
import { useRouter } from "vue-router";

const ticketStore = useTicketStore()
const router = useRouter()

const loaded = ref(false)
const ticket = ref({})

const load = async () => {
  // loaded.value = false
  const id = router.currentRoute.value.params.id
  ticket.value = await ticketStore.getTicketById({ticketId: id}) || {}
  loaded.value = true
}

onMounted(async () => {
  await load()
})

const AsyncTicket = defineAsyncComponent(() => import('components/ticket/Item.vue'))

</script>
