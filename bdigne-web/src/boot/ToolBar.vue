<template>
  <div class="row no-wrap shadow-1">
    <q-toolbar class="col-8">
      <q-btn
        flat dense round icon="menu"
        aria-label="Menu"
        @click="emits('toggleLeftDrawer')"
      />

      <div class="text-uppercase text-h5 q-mx-md">Jira Copy</div>

      <q-tabs v-model="tab" shrink>
        <q-tab name="/dash" label="DashBoard" @click="openPage()"/>
        <q-tab name="/project" label="Projects" @click="openPage()"/>
      </q-tabs>

      <q-input
        :model-value="searchVal"
        dense filled flat outlined
        label="Type here to search.." label-color="white" input-class="text-white"
      >
        <template v-slot:append>
          <q-btn flat round dense icon="search" color="white" @click="search()"/>
        </template>
      </q-input>
    </q-toolbar>

    <q-toolbar class="flex justify-end">
      <q-btn flat round dense icon="logout" color="white" @click="logout()"/>
    </q-toolbar>
  </div>
</template>

<script setup>
import {onMounted, ref} from "vue";
import { useRouter } from "vue-router";

const emits = defineEmits(['toggleLeftDrawer'])
const router = useRouter()

const tab = ref('/dash')
const searchVal = ref('')
const isLoggedIn = ref(false)

const openPage = () => {
  if (router.currentRoute.value.fullPath !== tab.value){
    router.replace(tab.value)
  }
}

const search = () => {

}
const logout = () => {
  localStorage.clear()
  isLoggedIn.value = false
  //TODO - api logout logic!!!
  router.replace('/')
}

//TODO - egy watcher ami figyeli a route-ot és az alapján beállítja a jelenlegi tabot

</script>
