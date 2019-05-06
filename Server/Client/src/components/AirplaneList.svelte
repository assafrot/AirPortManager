<script>
  export let airplanes;
  export let emptyListMessage = "No Airplanes";
</script>


<h2>Airplane in Queue</h2>

<ul>

{#if airplanes == undefined}
  <p>{emptyListMessage}</p>
{:else}
  
  {#each airplanes as airplane}
  <li>{airplane.name} {airplane.arriveTime.getHours()}:{airplane.arriveTime.getMinutes()}</li>
  {/each}

{/if}

</ul>