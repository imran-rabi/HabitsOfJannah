<template>
  <div class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white p-6 rounded-lg w-full max-w-md">
      <div class="flex justify-between items-center mb-4">
        <h3 class="text-lg font-semibold">Crop Profile Picture</h3>
        <button @click="$emit('close')" class="text-gray-500 hover:text-gray-700">
          <span class="text-2xl">&times;</span>
        </button>
      </div>

      <div class="relative w-full h-[400px] mb-4 bg-gray-100 rounded-lg overflow-hidden">
        <VueCropper
          ref="cropperRef"
          :src="imageUrl"
          :aspect-ratio="1"
          :view-mode="1"
          :drag-mode="'move'"
          :auto-crop-area="0.9"
          :background="true"
          :rotatable="false"
          :zoomable="true"
          :guide-lines="true"
          :center="true"
          :highlight="false"
          :crop-box-movable="true"
          :crop-box-resizable="false"
          :responsive="true"
          :modal="true"
        />
      </div>

      <div class="flex flex-col gap-4">
        <div class="flex items-center gap-2">
          <span class="text-sm text-gray-500">Zoom</span>
          <input
            type="range"
            min="0.5"
            max="3"
            step="0.1"
            :value="1"
            @input="onZoomChange"
            class="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer"
          >
        </div>
        
        <div class="flex justify-end gap-4">
          <button 
            @click="$emit('close')"
            class="px-4 py-2 text-gray-600 hover:text-gray-800"
          >
            Cancel
          </button>
          <button 
            @click="cropImage"
            class="px-4 py-2 bg-primary text-white rounded-lg hover:bg-opacity-90"
          >
            Save
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import VueCropper from 'vue-cropperjs'
import 'cropperjs/dist/cropper.css'

const props = defineProps<{
  imageUrl: string
}>()

const emit = defineEmits(['close', 'crop'])
const cropperRef = ref<any>(null)

const onZoomChange = (event: Event) => {
  const input = event.target as HTMLInputElement
  if (cropperRef.value) {
    cropperRef.value.zoomTo(parseFloat(input.value))
  }
}

const cropImage = () => {
  if (!cropperRef.value) return

  const canvas = cropperRef.value.getCroppedCanvas({
    width: 300,
    height: 300,
    imageSmoothingEnabled: true,
    imageSmoothingQuality: 'high'
  })

  canvas.toBlob((blob: Blob) => {
    if (blob.size > 5 * 1024 * 1024) {
      alert('Image size too large. Please choose a smaller image.')
      return
    }
    emit('crop', blob)
  }, 'image/jpeg', 0.9)
}
</script>

<style>
.cropper-view-box,
.cropper-face {
  border-radius: 50%;
}

.cropper-view-box {
  outline: 0;
  box-shadow: 0 0 0 1px #fff;
}

.cropper-face {
  background-color: inherit !important;
}

.cropper-modal {
  background-color: rgba(0, 0, 0, 0.5);
}

/* Style the range input */
input[type="range"] {
  @apply accent-primary;
}

input[type="range"]::-webkit-slider-thumb {
  @apply w-4 h-4 bg-primary rounded-full cursor-pointer appearance-none;
  margin-top: -4px;
}

input[type="range"]::-webkit-slider-runnable-track {
  @apply h-2 bg-gray-200 rounded-lg;
}
</style> 