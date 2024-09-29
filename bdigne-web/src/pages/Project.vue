<template>
  <q-page class="">
    <h4 v-if="loaded" class="q-my-md q-ml-sm">Projects</h4>

    <div>{{}}</div>

    <q-card class="full-width full-height">
<!--      TODO - amikor projektet váltunk triggereljen egy reloadot az child-componensekben-->
      <q-btn-dropdown
        v-if="loaded"
        :label="selectedProject.name"
        dense color="primary" menu-anchor="top start"
      >
        <q-item
          v-for="p in list" :key="p"
          clickable v-close-popup @click="setNewProject(p)"
        >
          <div>{{ p.name }}</div>
        </q-item>
      </q-btn-dropdown>

      <q-card-section :horizontal="true">
        <AsyncTicketList
          v-if="loaded === true && selectedProject !== null"
          :project="selectedProject"
          @selected="setNewTicket()"
        ></AsyncTicketList>

        <AsyncTicket
          v-if="selectedTicket"
          :ticket="selectedTicket"
          :project="selectedProject"
          @reloadNeeded="reloadTicket"
        ></AsyncTicket>
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from "vue";
import { useProjectStore } from "stores/project";
import { useTicketStore } from "stores/ticket";
import _ from "lodash";
import { useRouter } from "vue-router";

defineOptions({
  name: 'ProjectPage'
})

const router = useRouter()
const projectStore = useProjectStore()
const ticketStore = useTicketStore()

const list = ref([])
const loaded = ref(false)
const selectedProject = ref(null)
const selectedTicket = ref(null)

const load = async () => {
  await projectStore.getAll()
  const projects = _.cloneDeep(projectStore.projects) || []

  list.value = _.sortBy(projects, 'createdAt').reverse()

  router.currentRoute?.value.params?.id
    ? selectedProject.value = list.value.find(el => `${el.id}` ===  router.currentRoute?.value.params?.id)
    : selectedProject.value = list.value[0]

  loaded.value = true
}

const reloadTicket = async () => {
  selectedTicket.value = await ticketStore.getTicketById({ticketId: selectedTicket.value.id}) || {}
}

const setNewTicket = () => {
  selectedTicket.value = ticketStore.currentTicket
}

const setNewProject = (project) => {
  selectedProject.value = project
}

onMounted(async () => {
  await load()
})

const AsyncTicketList = defineAsyncComponent(() => import('../components/project/ticketList.vue'))
const AsyncTicket = defineAsyncComponent(() => import('../components/ticket/Item.vue'))

</script>
