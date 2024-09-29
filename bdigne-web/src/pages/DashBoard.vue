<template>
  <q-page class="">
    <h4 class="q-my-md q-ml-sm">Dashboard</h4>

    <div v-if="loaded" class="flex justify-center col no-wrap">
      <div>
        <AsyncUserRelatedTable :isAssignee="true"/>
        <AsyncUserRelatedTable :isAssignee="false"/>
      </div>
      <AsyncTicketTable/>
    </div>
    <q-separator class="q-mx-lg q-my-md"/>

    <div>Ide még jöhetnének majd cuccok</div>

    <q-page-sticky position="bottom-right" :offset="[18, 18]">
      <q-btn
        fab icon="add" color="primary"
        @click="createTicket"
      >
        <q-tooltip anchor="top middle" :offset="[25, 25]">
          Submit new ticket
        </q-tooltip>
      </q-btn>
    </q-page-sticky>

    <q-dialog v-model="isTicketDialogOpen" full-height full-width>
      <AsyncTicketDialog></AsyncTicketDialog>
    </q-dialog>

  </q-page>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from "vue";
import { useTicketStore } from "stores/ticket";

const ticketStore = useTicketStore()

const loaded = ref(false)
const isTicketDialogOpen = ref(false)

const load = async () => {
  await ticketStore.load()
  loaded.value = true
}

const createTicket = () => {
  toggleTicketDialog()
}

const toggleTicketDialog = () => {
  isTicketDialogOpen.value = !isTicketDialogOpen.value
}

onMounted(async () => {
  await load()
})

const AsyncUserRelatedTable = defineAsyncComponent(() => import('components/dashboard/UserRelatedTable.vue'))
const AsyncTicketTable = defineAsyncComponent(() => import('../components/dashboard/Table.vue'))
const AsyncTicketDialog = defineAsyncComponent(() => import('../components/dashboard/TicketDialog.vue'))
</script>
