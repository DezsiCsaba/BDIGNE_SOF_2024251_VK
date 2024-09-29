<template>
  <q-card class="q-ma-sm full-width" flat v-if="loaded">

<!--    header - title and buttons-->
    <q-card-section>
<!--      DEBUG-->
<!--      <div>{{ ticket }}</div>-->
      <q-card-section :horizontal="true">
        <q-img src="https://cdn.quasar.dev/img/parallax2.jpg" width="60px" height="60px" style="border-radius: 8px"/>
        <div class="q-ml-md">
          <div>
            <a :href="getProjectUrl()" target="_blank">{{ projectOfTicket.name }}</a> / <a :href="getCurrentUrl()">{{ getTicketName() }}</a>
          </div>
          <h6 class="q-my-xs">{{ ticket.title }}</h6>
        </div>
      </q-card-section>

      <div class="flex q-mt-md">
<!--        TODO - buttonGenerator bővítése!!! Kellene egy dropdown is-->
        <div v-if="ticket.reporterId === LocalStore.getUserData().id">
          <q-btn :label="'Edit'"/>
          <q-btn :label="'Reassign'"/>
        </div>

        <div v-if="LocalStore.getUserData().id === ticket.reporterId ||  LocalStore.getUserData().id === ticket.assigneeId">
          <q-btn-dropdown :label="ticket.status" :color="'primary'" :loading="!loaded">
            <q-list>
              <q-item v-for="state in stateOptions" :key="state"
                      clickable v-close-popup
                      @click="updateTicket({status: state, updateType: 'Status'})"
              >
                {{ state }}
              </q-item>
            </q-list>
          </q-btn-dropdown>
        </div>
      </div>
    </q-card-section>

<!--    body section of the ticket-->
    <q-card-section :horizontal="true" class="row">
      <div class="full-width">
        <AsyncClosableCardSection
          :title="'Details'"
          :ticket="ticket"
          :loadFn="loadFnForDetails"
          @loaded="console.log('done')"
        >
          <template v-slot:content>
            <div>
              Priority:
              <AsyncIconField
                :type="priorityToIcon('', '', ticket.priority)"
                :class="`text-${ticketStatusAndPriorityColorfn('', '', ticket.priority)} `"
                :tooltip="ticket.priority"
              ></AsyncIconField>
            </div>

          </template>
        </AsyncClosableCardSection>

        <AsyncClosableCardSection
          :title="'Description'"
          :ticket="ticket"
        >
          <template v-slot:content>
            <div>{{ ticket.description }}</div>
          </template>
        </AsyncClosableCardSection>

        <AsyncClosableCardSection
          :title="'Attachments'"
          :ticket="ticket"
        >
          <template v-slot:content>
<!--            TODO - egy attachments container-->
            <q-file dense outlined clearable label="Attach files"/>
          </template>
        </AsyncClosableCardSection>

        <AsyncClosableCardSection
          :title="'Activity'"
          :ticket="ticket"
        >
          <template v-slot:content>
            <AsyncActivityList
              v-if="componentInfoLoaded.activities"
              :ticket="ticket"
            ></AsyncActivityList>
          </template>
        </AsyncClosableCardSection>
      </div>
      <div class="col-4">
        <AsyncClosableCardSection
          :title="'People'"
          :ticket="ticket"
        >
          <template v-slot:content>
            <div>Assignee: {{ people?.assignee ?people.assignee.userName : 'None' }}</div>
            <div>Reporter: {{ people?.reporter ?people.reporter.userName : 'None' }}</div>
          </template>
        </AsyncClosableCardSection>

        <AsyncClosableCardSection
          :title="'Dates'"
          :ticket="ticket"
        >
          <template v-slot:content>
            <div>Created date: {{ toReadableDate(ticket.createdAt) }}</div>
            <div>Due date: {{ toReadableDate(ticket.dueDate) }}</div>
            <div>Last update: {{ toReadableDate(ticket.updatedAt) }}</div>

<!--            TODO - egy date picker ami mutatja mennyi idő van még a ticket deadline-ig?-->
          </template>
        </AsyncClosableCardSection>
      </div>
    </q-card-section>

  </q-card>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref, toRef, watch} from "vue";
import { useAuthStore } from "stores/auth";
import {
  getActivityEventTemplate,
  getCurrentUrl,
  priorityToIcon,
  ticketStatusAndPriorityColorfn,
  toReadableDate
} from "src/utils/functions";
import TicketItem from "components/ticket/Item.vue";
import { useProjectStore } from "stores/project";
import {useTicketStore} from "stores/ticket";
import {LocalStore} from "src/utils/helpers";

defineOptions({
  name: 'TicketItem'
})

const authStore = useAuthStore()
const projectStore = useProjectStore()
const ticketStore = useTicketStore()

const props = defineProps({
  ticket: {type: Object},
  getProjectUrl: {type: Object}
})

const emits = defineEmits(['reloadNeeded'])

const ticketRef = toRef(props, "ticket")
const stateOptions = ref(['InProgress', 'Done', 'Closed', 'Open'])
const userId = ref(null)
const people = ref({})
const projectOfTicket = ref(null)
const loaded = ref(false)
const componentInfoLoaded = ref({activities: false})

const configure = () => {
  // ['In Progress', 'Done', 'Closed', 'Open'].forEach(state => {
  //   if (state !== props.ticket.status) stateOptions.value.push(state)
  // })

  userId.value = authStore.userData ? authStore.userData.userId : localStorage.getItem('userId')
  projectOfTicket.value = props.project ? props.project : projectStore.currentProject

  people.value = {
    assignee: props.ticket.assignee,
    reporter: props.ticket.reporter
  }

}

const updateTicket = async (newVal = {}) => {
  let strippedTicket = {...props.ticket}
  strippedTicket.assignee = null
  strippedTicket.activities = null
  strippedTicket.project = null
  strippedTicket.reporter = null

  const payload = {
    userId: LocalStore.getUserData().id,
    ticketId: props.ticket.id,
    event: getActivityEventTemplate('ticketUpdate'),
    original: props.ticket[Object.keys(newVal)[0]],
    new: newVal[Object.keys(newVal)[0]],
    type: newVal.updateType,
    ticket: {...strippedTicket}
  }
  payload.ticket[Object.keys(newVal)[0]] = newVal[Object.keys(newVal)[0]]
  await ticketStore.updateTicket(payload)

  componentInfoLoaded.value.activities = false //ezt minden update esetén reloadolni akarjuk
  //TODO - többi componentre a newVal beérkező kulcai alapján el kellene dönteni, h min kell reloadot triggerelni
  emits('reloadNeeded')
}

function loadFnForDetails(data){
  console.log(data)
}

const getProjectUrl = () => {
  const id = props.project ? props.project.id : projectOfTicket.value.id
  return `${window.location.origin}/#/project/${id}`
}

const getTicketName = () => {
  return `${projectOfTicket.value.shortName}-${props.ticket.id}`
}

const load = async () => {
  if (props.project !== null){
    await projectStore.getById({projectId: props.ticket.projectId})
  }
  configure()

  //ha az adott componensnél van egy v-if ami figyeli akkor triggerelni fog egy reloadot rajta
  //kicsit csúnya megoldás, de ez az egyik legegyszerűbb módja, ha nem akar az ember reactive propokat injectelni...
  // TODO - ha van idő reactive propokra át kellene azért állni
  Object.keys(componentInfoLoaded.value).forEach(k => {
    if (componentInfoLoaded.value[k] === false){
      componentInfoLoaded.value[k] = true
    }
  })
  loaded.value = true
}

watch(ticketRef, async () => {
  await load()
})

onMounted(async () => {
  await load()
})

const AsyncClosableCardSection = defineAsyncComponent(() => import('../customBaseComponents/ClosableCardSection.vue'))
const AsyncIconField = defineAsyncComponent(() => import('../customBaseComponents/IconField.vue'))
const AsyncActivityList = defineAsyncComponent(() => import('./Activities.vue'))

</script>
