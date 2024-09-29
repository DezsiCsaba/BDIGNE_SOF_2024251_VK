<template>
  <q-card-section>
    <q-card-section :horizontal="true" class="flex items-center" :class="props.classes">
      <q-btn @click="toggleSection" size="xs" round flat :icon="isOpen ? 'close' : 'add'"/>

      <div class="text-bold">{{ title }}</div>
    </q-card-section>
    <div v-show="isOpen" class="q-mx-xl">
      <slot name="content"/>
    </div>
    <q-separator></q-separator>
  </q-card-section>
</template>

<script setup>
import {onMounted, ref} from "vue";

const props = defineProps({
  title: {type: String},
  ticket: {type: Object},
  classes: {type: String, default: 'q-ma-md q-mb-xs'},
  loadFn: {type: Function, default: null}
})
// const props = defineProps(['title', 'ticket', 'classes', 'loadFn'])
const emits = defineEmits(['loaded'])

const isOpen = ref(true)

const toggleSection = () => {
  isOpen.value = !isOpen.value
}

onMounted(async () => {
  if (props.loadFn !== null){
    await props.loadFn('fasz kivan :D')
    emits('loaded')
  }
})
</script>
