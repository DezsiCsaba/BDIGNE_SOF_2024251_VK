<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
<!--      <q-toolbar>-->
<!--        <q-btn-->
<!--          flat-->
<!--          dense-->
<!--          round-->
<!--          icon="menu"-->
<!--          aria-label="Menu"-->
<!--          @click="toggleLeftDrawer"-->
<!--        />-->

<!--        <q-toolbar-title>-->
<!--          Quasar App-->
<!--        </q-toolbar-title>-->

<!--        <div>Quasar v{{ $q.version }}</div>-->
<!--      </q-toolbar>-->
      <AsyncToolBar
      ></AsyncToolBar>
    </q-header>

<!--    TODO - saját drawer component-->
<!--    <q-drawer-->
<!--      v-model="leftDrawerOpen"-->
<!--      show-if-above-->
<!--      breakpoint="1600"-->
<!--      bordered-->
<!--    >-->
<!--      <q-list>-->

<!--      </q-list>-->
<!--    </q-drawer>-->

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import {defineAsyncComponent, onMounted, ref} from 'vue'
import {api} from "boot/axios";
import {Notify} from "quasar";

defineOptions({
  name: 'MainLayout'
})

const linksList = [
  {
    title: 'Docs',
    caption: 'quasar.dev',
    icon: 'school',
    link: 'https://quasar.dev'
  },
]

const leftDrawerOpen = ref(false)
const retryToConnectHappened = ref(false)

function toggleLeftDrawer () {
  leftDrawerOpen.value = !leftDrawerOpen.value
}

onMounted(async () => {
  await api({
    method: 'get',
    url: '/handshake'
  }).catch((err) => {
    console.error(err)
  })
})

const AsyncToolBar = defineAsyncComponent(() => import('../boot/ToolBar.vue'))

</script>
