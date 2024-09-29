<template>
  <q-table flat
    :columns="columns"
    :rows="rows"
    :pagination="pagination !== undefined ? pagination : { rowsPerPage: 10 }"
    :no-data-label="noDataLabel !== undefined ? noDataLabel : 'No data to show'"
  >
<!--    DEBUG-->
<!--    <template v-slot:top>-->
<!--      <slot name="topleft"></slot>-->
<!--      <div class="row q-mt-sm">-->
<!--        <div>{{ columns }}</div>-->
<!--      </div>-->
<!--    </template>-->
<!--    DEBUG-->

    <template v-slot:body="props">
      <q-tr :props="props">
        <q-td v-for="col in columns" :key="col.name" :class="col.classes" @click="console.log(col)">
          <template v-if="col.link">
            <a :href="getUrl(col.link, props.row.id)" target="_blank">{{ getColValue(col, props.row) }}</a>
          </template>
          <template v-else-if="col.iconField">
            <AsyncIconField
              :type="priorityToIcon(props.row, col)"
              :class="textColorFunction(col, props.row)"
              :tooltip="getColValue(col, props.row)"
            />
          </template>
          <template v-else>
            <div :class="textColorFunction(col,props.row)">{{ getColValue(col, props.row) }}</div>
          </template> <!-- default data view-->
        </q-td>
      </q-tr>
    </template>
  </q-table>
</template>

<script setup>
import { defineAsyncComponent } from "vue";
import { lowerCaseFirstLetter, priorityToIcon } from "src/utils/functions";

defineOptions({
  name: 'TableGenerator'
})

const props = defineProps([
  'columns', 'rows', 'rowsPerPage', 'pagination', 'noDataLabel'
])

function textColorFunction(col, row) {
  if (col.colorFn && typeof col.colorFn === 'function') {
    return `text-${col.colorFn(row, col)} `;
  }
}
function getColValue(col, row) {
  // if (col.field) {
  //   if (typeof col.field !== 'function') {
  //     return row[col.field];
  //   }
  // }
  return row[lowerCaseFirstLetter(col.name)];
}

function getUrl(link, id){
  // console.log(window.location.origin)
  return window.location.origin + '/#' + link + '/' + id
}


const AsyncIconField = defineAsyncComponent(() => import('./IconField.vue'))


</script>

