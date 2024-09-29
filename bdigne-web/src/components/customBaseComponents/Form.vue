<template>
  <q-form>
    <div v-for="input in inputs" :key="input" class="">

<!--      TODO - egy a táblázathoz hasonló icon function, hogy az input fieldre kerülhessen icon-->

      <q-input
        v-if="input.isQInput"
        v-model="input.refObj"
        :label="input.label"
        :type="input.inputType"
        :dense="input.dense"
        :filled="input.filled"
        :class="input.classes"
      >
        <template v-slot:before>
          <div v-if="input.mustHave" class="text-red flex flex-center">*</div>
          <div v-else class="invisible">*</div> <!--Hogy ne legyen probléma az alignmenttel-->
        </template>
        <template v-slot:prepend>
          <q-icon name="edit"></q-icon>
        </template>
      </q-input>


      <q-select
        v-if="input.isQSelect"
        :model-value="null"
        :options="input.refObj.value"
        :label="input.label"
        :dense="input.dense"
        :filled="input.filled"
        :class="input.classes"
      >
        <template v-slot:before>
          <div v-if="input.mustHave" class="text-red flex flex-center">*</div>
          <div v-else class="invisible">*</div> <!--Hogy ne legyen probléma az alignmenttel-->
        </template>
        <template v-slot:prepend>
          <q-icon name="edit"></q-icon>
        </template>
      </q-select>

    </div>


    <q-btn
      color="primary"
      @click="submit"
    >Submit</q-btn>
  </q-form>
</template>

<script setup>
import {onMounted, ref} from "vue";

defineOptions({
  name: 'FormGenerator'
})

const props = defineProps([
  'inputs'
])

const modelRefs = ref([])

const submit = () => {
  modelRefs.value.forEach(val => {
    console.log(val.value)
  })
}

const configure = () => {
  console.log({inputs: props.inputs})
  modelRefs.value = props.inputs.map((value) => ref(value))
}

onMounted(() => {
  configure()
})
</script>
