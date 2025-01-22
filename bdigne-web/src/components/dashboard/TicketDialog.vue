<template>
  <q-card>
    <q-card-section class="q-ma-none bg-primary">
      <div class="text-h5 text-white">Add your ticket</div>
    </q-card-section>

    <q-card-section class="q-pa-md">
<!--      <AsyncForm-->
<!--        :inputs="inputs"-->
<!--      ></AsyncForm>-->
      <q-form class="q-pa-md" v-if="loaded">
        <q-input
          v-model="title"
          label="Title"
          type="text"
        />
        <q-input
          v-model="description"
          label="Description"
          type="text"
        />

        <q-select
          v-model="prio"
          :options="priorityOpt"
          label="Priority"
        />
        <q-select
          v-model="status"
          :disable="true"
          label="Status"
        />
        <q-select
          v-model="assignee"
          :options="assigneeList.map((a) => a.userName)"
          label="Assignee"
        />
        <q-select
          v-model="project"
          :options="projectOpt.map((a) => a.name)"
          label="Project"
        />

        <q-btn
          class="q-mt-md"
          color="primary"
          @click="submit"
        >Submit</q-btn>
      </q-form>

    </q-card-section>


  </q-card>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from "vue";
import {formInputSetup, getInputConfigs} from "src/utils/formSetup";
import {useTicketStore} from "stores/ticket";
import {useProjectStore} from "stores/project";
import {useAuthStore} from "stores/auth";
import _ from "lodash";

const tStore = useTicketStore()
const pStore = useProjectStore()
const uStore = useAuthStore()

const priorityOpt = ['Low', 'Medium', 'High']
const statusOpt = ['Open', 'InProgress', 'Done', 'Closed']
let assigneeList = []
let projectOpt = []

const loaded = ref(false)
const title = ref('this is the title')
const description = ref('this is the descripton')
const prio = ref(priorityOpt[0])
const status = ref(statusOpt[0])
const assignee = ref(null)
const project = ref(null)

const assigneeObj = ref(['admin1', 'dev1'])
// const assignee = ref([assigneeObj])

const baseMap = {
  title: formInputSetup({ label: 'Title', refObj: title, mustHave: true }),
  description: formInputSetup({
    label: 'Description', refObj: description, inputType: 'textarea'
  }),
  assignee: formInputSetup({
    label: 'Assignee', refObj: assigneeObj, isQInput: false, isQSelect: true
  })
}
const inputs = [...getInputConfigs(baseMap)]

const submit = async () => {
  const payload = {
    // id: null,
    title: title.value,
    description: description.value,
    status: status.value,
    priority: prio.value,
    assigneeId: getAssigneeId(),
    reporterId: JSON.parse(localStorage.getItem('userData')).id || null,
    projectId: getProjectId(),
    dueDate: Date.now().toString(),
    createdAt: Date.now().toString(),
    updatedAt: Date.now().toString()
  }

  await tStore.create(payload)
}


const getProjectId = () => {
  return projectOpt.find(el => el.name === project.value)?.id || null
}
const getAssigneeId = () => {
  return assigneeList.find(el => el.userName === assignee.value)?.id || null
}

const load = async () => {
  await pStore.getAll()
  await uStore.getAll()

  projectOpt = _.cloneDeep(pStore.projects) || []
  assigneeList = _.cloneDeep(uStore.users).filter(u => u.role === 'Dev') || []

  assignee.value = assigneeList.map(u => u.userName)[0]
  project.value = projectOpt.map(p => p.name)[0]
}

onMounted(async () => {
  await load()
  loaded.value = true
})

const AsyncForm = defineAsyncComponent(() => import('components/customBaseComponents/Form.vue'))
</script>
