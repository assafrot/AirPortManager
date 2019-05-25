
<script>

  import { NodeGraphRenderer } from "../services/NodeGraphRenderer.ts";
  import { NodeGraph } from "../services/NodeGraph.ts"
  import { Node, NodeConnection } from "../models/Node.ts"
  import { AT_LANDING, AT_TAKEOFF } from "../models/Events.ts"
  import { AirportStateStore } from "../services/AirportService.ts"
  import { onMount } from "svelte";
  import { get } from "svelte/store";

  let canvas;
  let context;
  let renderer;


  onMount(() => {
    canvas.width = 800;
    canvas.height = 600;
    context = canvas.getContext("2d");
    renderer = new NodeGraphRenderer(context);
    renderer.drawNodeGraph($AirportStateStore.graph);

    AirportStateStore.subscribe(state => {
      renderer.drawNodeGraph($AirportStateStore.graph)
    })


});

  function onMouseMove(e) {
    let bounding = canvas.getBoundingClientRect();
    $AirportStateStore.graph.onCursorMoved({x: e.clientX - bounding.left,
                             y: e.clientY - bounding.top});
    
    renderer.drawNodeGraph($AirportStateStore.graph);
  }

</script>

<canvas bind:this={canvas} 
        on:mousemove={onMouseMove}
        on:mousedown={() => $AirportStateStore.graph.onCursorPressed()}
        on:mouseup={() => $AirportStateStore.graph.onCursorReleased()}>
</canvas>