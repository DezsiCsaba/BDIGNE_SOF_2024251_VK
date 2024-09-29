<template>
  <q-page class="flex flex-center">
    <q-card style="max-width: 400px; width: 100%;">
      <q-card-section>
        <div class="text-h6 q-mb-md">Login</div>

        <q-input
          filled
          v-model="userName"
          label="Username"
          type="text"
          :rules="[val => !!val || 'Username is required']"
        />

        <q-input
          filled
          v-model="password"
          label="Password"
          type="password"
          :rules="[val => !!val || 'Password is required']"
          class="q-mt-md"
        />

        <q-btn
          label="Login"
          color="primary"
          class="full-width q-mt-md"
          :loading="loading"
          @click="login"
        />
      </q-card-section>
    </q-card>
  </q-page>
</template>

<script setup>
import {defineAsyncComponent, ref} from "vue";
import {useRouter} from "vue-router";
import {useTestStore} from "stores/test";
import {useAuthStore} from "stores/auth";
import {Notify} from "quasar";

defineOptions({
  name: 'IndexPage'
});

//stores && other consts
const testStore = useTestStore()
const authStore = useAuthStore()
const router = useRouter()

//refs
const userName = ref('admin')
const password = ref('AdminPass123')
const loading = ref(false)

//functions
const login = async() => {
  loading.value = true
  await authStore.auth({Email: userName.value, UserName: userName.value, Password: password.value})
    .then(async () => {
      loading.value = false
      await router.push('/dash')
    })
}

const AsyncBtn = defineAsyncComponent(() => import('../components/customBaseComponents/ButtonGenerator.vue'))
</script>
