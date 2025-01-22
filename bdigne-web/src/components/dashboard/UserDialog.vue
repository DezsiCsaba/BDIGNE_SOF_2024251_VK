<template>
  <q-card>
    <q-card-section class="q-ma-none bg-primary">
      <div class="text-h5 text-white">Add your ticket</div>
    </q-card-section>

    <q-card-section class="q-pa-md">
      <div>Please fill out every field marked with <span class="text-red">*</span> and then submit the form to register the new user</div>

<!--      <AsyncForm-->
<!--        :inputs="inputs"-->
<!--      ></AsyncForm>-->
      <q-form class="q-pa-md">
        <q-input
          v-model="userName"
          label="Username"
          type="text"
        />

        <q-input
          v-model="email"
          label="Email"
          type="text"
        />

        <q-select
          v-model="userRole"
          :options="userRoles"
          label="User's role"
        />

        <q-btn
          class="q-mt-md"
          color="primary"
          @click="submit"
        >Submit</q-btn>
      </q-form>
    </q-card-section>
  </q-card>
</template>

<script setup>
import {defineAsyncComponent, ref} from "vue";
import {formInputSetup, getInputConfigs} from "src/utils/formSetup";
import {useAuthStore} from "stores/auth";

const uStore = useAuthStore()

const userName = ref('testUser')
const email = ref('testemail@gmail.com')
const userRoles = ["Admin", "Reporter", "Developer"]
const userRole = ref(userRoles[2])

const submit = async () => {
  const payload = {
    id: null,
    userName: userName.value,
    role: userRole.value,
    email: email.value,
    password: 'testPass123'
  }

  await uStore.register(payload)
}

const baseMap = {
  userName: formInputSetup({
    label: 'Username', refObj: userName, mustHave: true
  }),
  userRole: formInputSetup({
    label: "User's role", refObj: userRoles, isQInput: false, isQSelect: true
  })
}
const inputs = [...getInputConfigs(baseMap)]

const AsyncForm = defineAsyncComponent(() => import('components/customBaseComponents/Form.vue'))

</script>
