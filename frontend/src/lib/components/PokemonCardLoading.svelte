<div class="outer">
  <div class="wrapper">
    <div class="circle"></div>
    <div class="line flex-center">
      <div class="button-wrapper flex-center"><div class="button"></div></div>
    </div>
    <div class="half-circle"></div>
  </div>
</div>

<style>
  div.outer {
    width: 130px;
    height: 200px;
    background-color: blue;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: 1em;
  }

  div.wrapper {
    --size: 100px;
    width: var(--size);
    height: var(--size);
    position: relative;
  }
  div.circle {
    background-color: red;
    width: 100%;
    height: 100%;
    border-radius: 100%;
  }
  div.line {
    width: 100%;
    height: 1em;
    background-color: black;
    position: absolute;
    top: 50%;
    transform: translate(0, -50%);
    z-index: 5;
  }
  div.button-wrapper {
    --size: 2em;
    width: var(--size);
    height: var(--size);
    background-color: black;
    border-radius: 100%;
  }
  div.button {
    --size: 70%;
    width: var(--size);
    height: var(--size);
    background-color: white;
    border-radius: 100%;
    outline: 1px solid black;
    outline-offset: -4px;
  }
  .flex-center {
    display: flex;
    justify-content: center;
    align-items: center;
  }
  .half-circle {
    height: 50%;
    width: 100%;
    border-radius: 0 0 100em 100em;
    background-color: white;
    position: absolute;
    top: 50%;
    z-index: 1;
  }
</style>
