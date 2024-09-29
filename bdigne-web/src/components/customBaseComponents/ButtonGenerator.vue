<template>
  <q-btn
    v-if="btnConfMap.icon"
    :class="btnConfMap.class"
    :style="btnConfMap.style"
  ></q-btn>

  <q-btn
    v-else
    :class="btnConfMap.class"
    :style="btnConfMap.style"
  ></q-btn>
</template>
<script setup>
import {reactive, watchEffect} from "vue";
import {toBool} from "src/utils/functions";

const props = defineProps([
  'conf', 'class', 'type', 'classBase', 'label', 'color', 'style', 'textColor', 'size', 'round', 'toolTip'
])
const btnConfMap = reactive({})
const confs = reactive({
  basic: {
    class: '',
    style: '',
    label: '',
    icon: '',
    color: 'primary',
    textColor: null,
    size: '',
    round: false,
    tooltip: null
  },

  round: {class: 'q-mr-sm', size: 'sm', round: true}
})

watchEffect(() => {
  let conf = confs['basic']
  let tConf = props.conf || props.type

  if (confs[tConf]) {
    Object.assign(conf, confs[tConf])
  }

  let btnClass = props.classBase || props.class
  if (props.class){
    btnClass += ' ' + props.class
  }

  Object.assign(btnConfMap, {
    class: btnClass,
    style: props.style || conf.style,
    label: props.label,
    icon: props.icon || conf.icon,
    color: props.color,
    textColor: props.textColor || conf.textColor,
    size: props.size || conf.size,
    round: props.round ? toBool(props.round) : conf.round,
    tooltip: props.toolTip || conf.tooltip
  })
})
</script>
