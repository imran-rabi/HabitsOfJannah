<template>
  <Navbar v-if="auth.isAuthenticated" />
  <router-view></router-view>
</template>

<script setup lang="ts">
import { useAuth } from '@/stores/auth'
import Navbar from '@/components/layout/Navbar.vue'

const auth = useAuth()
</script>