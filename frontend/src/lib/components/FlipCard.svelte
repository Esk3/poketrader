<script lang="ts">
const {color, width, height, delay, children}: any = $props();
</script>

<div class="flip-card" style="width: {width || "100%" }; height: {height || "100%"};">
  <div class="flip-card-inner" style="animation-delay: {delay || 0}ms;">
    <div class="flip-card-front" style="background-color: {color}"> </div>
    <div class="flip-card-back" >
      {@render children?.()}
    </div>
  </div>
</div>

 <style>

.flip-card {
  background-color: transparent;
  perspective: 1000px;
}

.flip-card-inner {
  position: relative;
  width: 100%;
  height: 100%;
  text-align: center;
  transform-style: preserve-3d;
  animation: flip 2s forwards;
}

.flip-card-front, .flip-card-back {
  position: absolute;
  width: 100%;
  height: 100%;
  -webkit-backface-visibility: hidden; /* Safari */
  backface-visibility: hidden;
}

@keyframes flip {
from {
  transform: rotateY(0);
}
to {
  transform: rotateY(180deg);
}
}

.flip-card-back {
  transform: rotateY(180deg);
} 
</style>
