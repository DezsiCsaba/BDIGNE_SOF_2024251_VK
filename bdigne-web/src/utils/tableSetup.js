import { upperFirstLetter } from '../utils/functions'

export function getColumns(dataConfMap) {
  let res = []
  Object.entries(dataConfMap).forEach(([k, v]) => {
    res.push({
      name: v.tableSetup.name || k,
      field: v.tableSetup.field || k,
      label: v.tableSetup.label || upperFirstLetter(k),
      align: 'center',
      colorFn: v.tableSetup.colorFn || k,
      link: v.tableSetup.link || null,
      classes: v.tableSetup.classes || def.classes,
      iconField: v.tableSetup.iconField || def.iconField,
    })
  })
  return res
}

const def = {
  name: '',
  field: undefined,
  label: '',
  colorFn: undefined,
  link: undefined,
  classes: 'text-left',
  iconField: undefined,
}

// export const tableSetup = (
//   name=def.name,
//   field = def.field,
//   label = def.label,
//   colorFn = def.colorFn,
//   link = def.link,
//   classes = def.classes,
//   iconField = def.iconField
// ) => {
//   return {
//     tableSetup: { name, field, label,  align: 'center', colorFn, link, classes, iconField }
//   }
// }

export const tableSetup = ({
  name=def.name,
  field = def.field,
  label = def.label,
  colorFn = def.colorFn,
  link = def.link,
  classes = def.classes,
  iconField = def.iconField
} = {}) => {
  return {
    tableSetup: { name, field, label,  align: 'center', colorFn, link, classes, iconField }
  }
}
