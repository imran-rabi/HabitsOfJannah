<template>
  <Navbar v-if="isAuthenticated" />
  <router-view></router-view>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import Navbar from './components/layout/Navbar.vue'

const isAuthenticated = computed(() => !!localStorage.getItem('token'))
</script>