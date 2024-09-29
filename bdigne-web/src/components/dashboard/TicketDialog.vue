<template>
  <q-card>
    <q-card-section class="q-ma-none bg-primary">
      <div class="text-h5 text-white">Add your ticket</div>
    </q-card-section>

    <q-card-section class="q-pa-md">
      <div>Please fill out every field marked with <span class="text-red">*</span> and then submit your ticket</div>

      <AsyncForm
        :inputs="inputs"
      ></AsyncForm>

    </q-card-section>


  </q-card>
</template>

<script setup>
import {defineAsyncComponent, ref} from "vue";
import {formInputSetup, getInputConfigs} from "src/utils/formSetup";

const title = ref('this is the title')
const description = ref('this is the descripton')
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

const AsyncForm = defineAsyncComponent(() => import('components/customBaseComponents/Form.vue'))
</script>
