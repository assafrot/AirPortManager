<script> 
  import { AirportStateStore } from "../services/AirportService"
  import { onMount } from "svelte";
  import { get } from "svelte/store";

  function formatDate(date) {
      return date.toLocaleString();
  } 

</script>

<style>
    .list {
        display: flex;
        flex-direction:  column;
        background: lightgrey;
        margin: 2rem auto; 
        max-width: 800px;
    }

    .row {
        display: grid;
        grid-template-columns:  repeat(3, 1fr);
    }

    .row:nth-child(even) {
        background: white;

    }

    .row:nth-child(odd) {
        background: lightgrey;

    }

    .row:first-child {
        background: darkgreen;
        color:white;
    }

    .row p {
        text-align: center;
    }
</style>

<div class="list">
    <div class="row">
        <p>Flight Number</p>
        <p>Request Time</p>
        <p>Action Type</p>
    </div>
	{#each $AirportStateStore.airplanesInQueue as item}
		<div class="row">
            <p>{item.flightNumber}</p>
            <p>{formatDate(item.requestTime)}</p>
            <p>{item.actionType}</p>
        </div>
	{/each}
</div>