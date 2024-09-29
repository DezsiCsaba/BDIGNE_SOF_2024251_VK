<template>
  <div> Ticket activities: </div>

<!--  <div>%user% made changes - %date%<br><span class="text-grey">%type%</span><b>Original:</b>%original%, <b>New:</b>%new%</div>-->

  <q-icon></q-icon>
  <div v-if="load">
    <q-card class="q-pa-xs" flat v-for="a in list" :key="a">
<!--      <div>{{a.event}}</div><br>-->
      <div v-html="a.event"></div>
      <q-separator></q-separator>
    </q-card>
  </div>

<!--  <a :href="getCurrentUrl()">{{ getTicketName() }}</a>-->
</template>

<script setup>
import {onMounted, ref} from "vue";
import _ from "lodash";
import {LocalStore} from "src/utils/helpers";
import {getCurrentUrl, toReadableDate} from "src/utils/functions";

defineOptions({
  name: 'TicketActivities'
})

const props = defineProps(['ticket'])

const list = ref([])
const loaded = ref(false)

const load = () => {
  list.value = _.sortBy(_.cloneDeep(props.ticket.activities), 'createdAt').reverse() || []
  mapEventStrings()
  loaded.value = true
}

const mapEventStrings = () => {
  list.value.map(activity => {
    activity.event = activity.event
      .replace('%user%', getUserNameAndProfilePic(activity))
      .replace('%date%', toReadableDate(activity.createdAt))
      .replace('%type%', getType(activity))
      .replace('%original%', activity.original)
      .replace('%new%', activity.new)
  })
}
const getUserNameAndProfilePic = (activity) => {
  return `
  <img class="q-avatar" style="width: 20px; height: 20px" src="https://cdn.quasar.dev/img/boy-avatar.png">
  @${activity.user.userName}` //TODO - prof kép megjelenítése
}
const getType = (activity) => {
  return `<span class="text-caption text-grey">${activity.type}</span>`
}

onMounted( () => {
  load()
})

</script>
