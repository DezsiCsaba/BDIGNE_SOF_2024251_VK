import {toRef} from "vue";

export function getInputConfigs(dataConfMap) {
  let res = []
  Object.entries(dataConfMap).forEach(([k, v]) => {
    console.log(k,v)
    res.push({
      //basic stuff (ref for inputs model-value, label...
      label: v.formInputSetup.label || k,
      refObj: v.formInputSetup.refObj || k,
      inputType: v.formInputSetup.inputType || def.inputType,
      mustHave: v.formInputSetup.mustHave || def.mustHave,

      //arra van, h ha esetleg mást akarunk használni mint a sima q-inut (dropdown, select...) akarunk használni
      isQInput: v.formInputSetup.isQInput !== undefined ? v.formInputSetup?.isQInput : def.isQInput,
      isQSelect: v.formInputSetup.isQSelect || def.isQSelect,

      //other, mainly style related stuff
      dense: v.formInputSetup.dense || def.dense,
      filled: v.formInputSetup.filled || def.filled,
      classes: v.formInputSetup.classes || def.classes,
    })
  })
  return res
}

const def = {
  label: '',
  refObj: toRef(''),
  inputType: 'text',
  mustHave: false,

  isQInput: true,
  isQSelect: false,

  dense: true,
  filled: true,
  classes: 'q-ma-sm',
}

export const formInputSetup = ({
  label = def.label,
  refObj = def.refObj,
  inputType = def.inputType,
  mustHave = def.mustHave,

  isQInput = def.isQInput,
  isQSelect = def.isQSelect,

  dense = def.dense,
  filled = def.filled,
  classes = def.classes
} = {}) => {
  return {
    formInputSetup: { label, refObj, inputType, mustHave, isQInput, isQSelect, dense, filled, classes }
  }
}
