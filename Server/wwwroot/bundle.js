
(function(l, i, v, e) { v = l.createElement(i); v.async = 1; v.src = '//' + (location.host || 'localhost').split(':')[0] + ':35729/livereload.js?snipver=1'; e = l.getElementsByTagName(i)[0]; e.parentNode.insertBefore(v, e)})(document, 'script');
var app = (function () {
	'use strict';

	function noop() {}

	function add_location(element, file, line, column, char) {
		element.__svelte_meta = {
			loc: { file, line, column, char }
		};
	}

	function run(fn) {
		return fn();
	}

	function blank_object() {
		return Object.create(null);
	}

	function run_all(fns) {
		fns.forEach(run);
	}

	function is_function(thing) {
		return typeof thing === 'function';
	}

	function safe_not_equal(a, b) {
		return a != a ? b == b : a !== b || ((a && typeof a === 'object') || typeof a === 'function');
	}

	function validate_store(store, name) {
		if (!store || typeof store.subscribe !== 'function') {
			throw new Error(`'${name}' is not a store with a 'subscribe' method`);
		}
	}

	function subscribe(component, store, callback) {
		const unsub = store.subscribe(callback);

		component.$$.on_destroy.push(unsub.unsubscribe
			? () => unsub.unsubscribe()
			: unsub);
	}

	function append(target, node) {
		target.appendChild(node);
	}

	function insert(target, node, anchor) {
		target.insertBefore(node, anchor);
	}

	function detach(node) {
		node.parentNode.removeChild(node);
	}

	function destroy_each(iterations, detaching) {
		for (let i = 0; i < iterations.length; i += 1) {
			if (iterations[i]) iterations[i].d(detaching);
		}
	}

	function element(name) {
		return document.createElement(name);
	}

	function text(data) {
		return document.createTextNode(data);
	}

	function space() {
		return text(' ');
	}

	function listen(node, event, handler, options) {
		node.addEventListener(event, handler, options);
		return () => node.removeEventListener(event, handler, options);
	}

	function children(element) {
		return Array.from(element.childNodes);
	}

	function set_data(text, data) {
		data = '' + data;
		if (text.data !== data) text.data = data;
	}

	let current_component;

	function set_current_component(component) {
		current_component = component;
	}

	function get_current_component() {
		if (!current_component) throw new Error(`Function called outside component initialization`);
		return current_component;
	}

	function onMount(fn) {
		get_current_component().$$.on_mount.push(fn);
	}

	const dirty_components = [];

	const resolved_promise = Promise.resolve();
	let update_scheduled = false;
	const binding_callbacks = [];
	const render_callbacks = [];
	const flush_callbacks = [];

	function schedule_update() {
		if (!update_scheduled) {
			update_scheduled = true;
			resolved_promise.then(flush);
		}
	}

	function add_binding_callback(fn) {
		binding_callbacks.push(fn);
	}

	function add_render_callback(fn) {
		render_callbacks.push(fn);
	}

	function flush() {
		const seen_callbacks = new Set();

		do {
			// first, call beforeUpdate functions
			// and update components
			while (dirty_components.length) {
				const component = dirty_components.shift();
				set_current_component(component);
				update(component.$$);
			}

			while (binding_callbacks.length) binding_callbacks.shift()();

			// then, once components are updated, call
			// afterUpdate functions. This may cause
			// subsequent updates...
			while (render_callbacks.length) {
				const callback = render_callbacks.pop();
				if (!seen_callbacks.has(callback)) {
					callback();

					// ...so guard against infinite loops
					seen_callbacks.add(callback);
				}
			}
		} while (dirty_components.length);

		while (flush_callbacks.length) {
			flush_callbacks.pop()();
		}

		update_scheduled = false;
	}

	function update($$) {
		if ($$.fragment) {
			$$.update($$.dirty);
			run_all($$.before_render);
			$$.fragment.p($$.dirty, $$.ctx);
			$$.dirty = null;

			$$.after_render.forEach(add_render_callback);
		}
	}

	function mount_component(component, target, anchor) {
		const { fragment, on_mount, on_destroy, after_render } = component.$$;

		fragment.m(target, anchor);

		// onMount happens after the initial afterUpdate. Because
		// afterUpdate callbacks happen in reverse order (inner first)
		// we schedule onMount callbacks before afterUpdate callbacks
		add_render_callback(() => {
			const new_on_destroy = on_mount.map(run).filter(is_function);
			if (on_destroy) {
				on_destroy.push(...new_on_destroy);
			} else {
				// Edge case - component was destroyed immediately,
				// most likely as a result of a binding initialising
				run_all(new_on_destroy);
			}
			component.$$.on_mount = [];
		});

		after_render.forEach(add_render_callback);
	}

	function destroy(component, detaching) {
		if (component.$$) {
			run_all(component.$$.on_destroy);
			component.$$.fragment.d(detaching);

			// TODO null out other refs, including component.$$ (but need to
			// preserve final state?)
			component.$$.on_destroy = component.$$.fragment = null;
			component.$$.ctx = {};
		}
	}

	function make_dirty(component, key) {
		if (!component.$$.dirty) {
			dirty_components.push(component);
			schedule_update();
			component.$$.dirty = {};
		}
		component.$$.dirty[key] = true;
	}

	function init(component, options, instance, create_fragment, not_equal$$1, prop_names) {
		const parent_component = current_component;
		set_current_component(component);

		const props = options.props || {};

		const $$ = component.$$ = {
			fragment: null,
			ctx: null,

			// state
			props: prop_names,
			update: noop,
			not_equal: not_equal$$1,
			bound: blank_object(),

			// lifecycle
			on_mount: [],
			on_destroy: [],
			before_render: [],
			after_render: [],
			context: new Map(parent_component ? parent_component.$$.context : []),

			// everything else
			callbacks: blank_object(),
			dirty: null
		};

		let ready = false;

		$$.ctx = instance
			? instance(component, props, (key, value) => {
				if ($$.ctx && not_equal$$1($$.ctx[key], $$.ctx[key] = value)) {
					if ($$.bound[key]) $$.bound[key](value);
					if (ready) make_dirty(component, key);
				}
			})
			: props;

		$$.update();
		ready = true;
		run_all($$.before_render);
		$$.fragment = create_fragment($$.ctx);

		if (options.target) {
			if (options.hydrate) {
				$$.fragment.l(children(options.target));
			} else {
				$$.fragment.c();
			}

			if (options.intro && component.$$.fragment.i) component.$$.fragment.i();
			mount_component(component, options.target, options.anchor);
			flush();
		}

		set_current_component(parent_component);
	}

	class SvelteComponent {
		$destroy() {
			destroy(this, true);
			this.$destroy = noop;
		}

		$on(type, callback) {
			const callbacks = (this.$$.callbacks[type] || (this.$$.callbacks[type] = []));
			callbacks.push(callback);

			return () => {
				const index = callbacks.indexOf(callback);
				if (index !== -1) callbacks.splice(index, 1);
			};
		}

		$set() {
			// overridden by instance, if it has props
		}
	}

	class SvelteComponentDev extends SvelteComponent {
		constructor(options) {
			if (!options || (!options.target && !options.$$inline)) {
				throw new Error(`'target' is a required option`);
			}

			super();
		}

		$destroy() {
			super.$destroy();
			this.$destroy = () => {
				console.warn(`Component was already destroyed`); // eslint-disable-line no-console
			};
		}
	}

	/* src\components\AirplaneList.svelte generated by Svelte v3.1.0 */

	class NodeGraph {

	  constructor() {
	    this.nodes = [];
	    this.cursor = {x : 0,y : 0};
	    this.movingNodeIdx = -1;
	    this.nodeConnectionList = this.generateConnectionList();
	    this.connections = this.generateConnections();
	    this.style = {
	      canvas : {
	        background : "white"
	      },
	      node: {
	        colors: {
	          default: "lightgrey",
	          startPoint: "green",
	          endPoint: "Pink"
	        }
	      },
	      connection:{
	        lineWidth: 4,
	        lineMargin: 10,
	        arrowSize: 20,
	        colors : {
	          border: "black",
	          AT_LANDING:"red",
	          AT_TAKEOFF:"blue"
	        } 
	      }
	    };
	  }

	  generateLines() {
	    
	    let nodeSideCons = [];

	    this.nodes.forEach((node,idx) => {
	      nodeSideCons[idx] = [];
	    });

	    this.nodes.forEach((node,idx) => {
	      node.connections.forEach(connection => {
	        let dir = this.getDir(node, connection.node);
	        
	        if(nodeSideCons[idx][dir.from]) {
	          nodeSideCons[idx][dir.from]++;
	        } else {
	          nodeSideCons[idx][dir.from] = 1;
	        }

	        let secondNodeIdx = this.nodes.indexOf(connection.node);
	        
	        if(nodeSideCons[secondNodeIdx][dir.to]) {
	          nodeSideCons[secondNodeIdx][dir.to]++;
	        } else {
	          nodeSideCons[secondNodeIdx][dir.to] = 1;
	        }

	      });
	    });

	    console.log(nodeSideCons);
	    
	  }

	  generateConnectionList() {
	    let list = [];

	    this.nodes.forEach((node, idx) => {

	      node.connections.forEach(connection => {
	        
	        let cNodeIdx = this.nodes.indexOf(connection.node);
	        let gConnection = list.find(con => con.nodeLeft == connection.node && con.nodeRight == node);

	        if(idx > cNodeIdx && gConnection) {
	            
	          gConnection.connections.push({
	            from: node,
	            to: connection.node,
	            type: connection.type
	          });

	        } else {
	            
	          list.push({
	            nodeLeft: node, 
	            nodeRight: connection.node, 
	            connections: [{
	              from: node,
	              to: connection.node,
	              type: connection.type
	            }]
	          });

	        }

	    });});

	    return list;
	  }

	  generateConnections() {
	    let connections = [];

	    this.nodeConnectionList.forEach(graphConnection => {
	      graphConnection.connections.forEach((connection,idx) => {
	        
	        let dir = getDir(connection.from, connection.to);
	        let offset = this.calcOffset(idx, graphConnection.connections.length);
	        
	        connections.push({
	          dir : dir
	          ,offset : offset
	          ,pointFrom : this.getConnectionPoint(connection.from, dir.from, offset)
	          ,pointTo : this.getConnectionPoint(connection.to, dir.to, offset)
	          ,color : this.style.connection.colors[connection.type]
	        });
	      });
	    });

	    return connections;
	  }

	  addNode(node) {
	    this.nodes.push(node);
	    this.nodeConnectionList = this.generateConnectionList();
	    this.connections = this.generateConnections();
	  }

	  addNodes(nodes) {
	    this.nodes.push(...nodes);
	    this.nodeConnectionList = this.generateConnectionList();
	    this.connections = this.generateConnections();
	  }
	  
	  removeNode(node) {
	    let idx = this.nodes.indexOf(node);
	    this.nodes.splice(idx,1);
	    this.nodeConnectionList = this.generateConnectionList();
	    this.connections = this.generateConnections();
	  }

	  onCursorPressed() {
	    let hoveredNode = getHoveredNode(this.nodes, this.cursor);
	    
	    if(hoveredNode) {
	      this.movingNodeIdx = this.nodes.indexOf(hoveredNode);
	    } else {
	      this.movingNodeIdx = -1;
	    }

	  }

	  onCursorReleased() {
	    this.movingNodeIdx = -1;
	  }

	  onCursorMoved(position) {
	    
	    this.nodes.forEach(node => {
	      node.hovered = isCursorHovering(node, this.cursor);
	    });
	    
	    if(this.movingNodeIdx > -1) {
	      this.nodes[this.movingNodeIdx].x += position.x - this.cursor.x;
	      this.nodes[this.movingNodeIdx].y += position.y - this.cursor.y;
	      this.connections = this.generateConnections();
	    }
	    
	    this.cursor = position;
	  }

	  calcOffset(idx, connectionCount) {
	    return (this.style.connection.lineWidth * ((-connectionCount / 2) + idx + 0.5)) +
	    (this.style.connection.lineMargin * (idx - 0.5))
	  }

	  getConnectionPoint(node, dir, offset) {
	    let lineWidth = this.style.connection.lineWidth;
	    switch(dir) {
	      case 0: 
	        return {x: node.x + node.width, y: node.y + (node.height / 2) + offset}
	        break;
	      case 1: 
	        return {x: node.x + (node.width / 2) + offset, y: node.y + node.height}
	        break;
	      case 2: 
	        return {x: node.x , y: node.y + (node.height / 2) + offset}
	        break;
	      case 3: 
	        return {x: node.x + (node.width / 2) + offset, y: node.y}
	        break;
	    }
	  }

	}

	function getDir(nodeFrom, nodeTo) {

	  if(Math.abs(nodeFrom.x - nodeTo.x) > 
	     Math.abs(nodeFrom.y - nodeTo.y)) {
	      if(nodeFrom.x > nodeTo.x) {
	        return {from: 2, to: 0 }
	      } else {
	        return {from: 0, to: 2 }
	      } 
	    } else {
	      if(nodeFrom.y > nodeTo.y) {
	        return {from: 3, to: 1 }
	      } else {
	        return {from: 1, to: 3 }
	      } 
	  }

	}

	function isCursorHovering(node, cursor) {
	  return(between(cursor.x, node.x, node.x + node.width) &&
	         between(cursor.y, node.y, node.y + node.height));
	}

	function between(num , min, max) {
	  return num >= min && num <= max;
	}

	function getHoveredNode(nodes, cursor) {
	  let hoveredNode;

	  nodes.forEach(node => {
	    if(isCursorHovering(node, cursor)) {
	      hoveredNode = node;
	    }
	  });

	  return hoveredNode;
	}

	class NodeGraphRenderer {

	  constructor(ctx) {
	    this.ctx = ctx;
	    this.style = {};
	  }

	  drawNodeGraph(graph) {
	    if(graph) {

	      this.style = graph.style;
	      
	      this.ctx.clearRect(0,0,800,600);
	      this.ctx.beginPath();
	      this.ctx.fillStyle =  this.style.canvas.background;
	      this.ctx.fillRect(0, 0, 800, 600);
	      this.ctx.fill();
	      
	      if(graph.connections) {
	        this.drawConnections(graph);
	      }
	      
	      if(graph.nodes) {
	        graph.nodes.forEach(node => {
	          this.drawNode(node);  
	        });
	      }
	      
	    }
	  }

	  drawNode(node) {
	    this.ctx.beginPath();
	    this.ctx.fillStyle = this.style.node.colors.default;
	    if(node.isStartPoint) {
	      this.ctx.fillStyle = this.style.node.colors.startPoint;
	    }
	    if(node.isEndPoint) {
	      this.ctx.fillStyle = this.style.node.colors.endPoint;
	    }
	    this.ctx.fillRect(node.x, node.y, node.width, node.height); 
	    this.ctx.fill();

	    this.ctx.beginPath();
	    this.ctx.fillStyle = "black";
	    this.ctx.fillText(node.id,node.x,node.y);
	    this.ctx.fill();     
	    // if(node.hovered) {
	    //   this.drawOutlineForNode(node);
	    // }
	  
	    // if(node.airplane) {
	    //   this.drawAirplaneInNode(node);
	    // }
	  }

	  drawOutlineForNode(node) {
	    this.ctx.beginPath();
	    this.ctx.moveTo(node.x, node.y);
	    this.ctx.strokeStyle = "lightgreen";
	    this.ctx.strokeRect(node.x, node.y, node.width, node.height);
	    this.ctx.stroke();
	  }

	  drawAirplaneInNode(node) {
	    this.ctx.beginPath();
	    this.ctx.fillStyle = "orange";

	    let querterWidth = (node.width/4);
	    let querterHeight = (node.height/4);

	    this.ctx.moveTo(node.x + querterWidth, node.y + querterHeight);
	    this.ctx.fillRect(node.x + querterWidth, node.y + querterHeight,
	                      querterWidth*2, querterHeight*2);
	    this.ctx.fill();
	  }

	  drawConnections(graph) {
	    
	    graph.connections.forEach(connection => {
	      this.drawConnection(connection.pointFrom, connection.pointTo, connection.color);
	    });
	    
	    graph.connections.forEach(connection => {
	      this.drawArrow(connection.pointFrom, connection.pointTo, connection.dir, connection.color);
	    });

	  }

	  drawArrow(pointFrom, pointTo, dir, color) {
	    let m = this.calcIncine(pointFrom, pointTo);
	    let arrowPoint = this.calcArrowPosition(pointTo, pointFrom, dir.to, m);
	    let angle = -this.calcDegree(pointFrom, pointTo) + 90;

	    this.drawTriangle(arrowPoint, angle,
	    this.style.connection.arrowSize,color);
	  }

	  calcIncine(pointA, pointB) {
	    return (pointA.y - pointB.y) / (pointA.x - pointB.x);
	  }

	  calcDegree(pointFrom, pointTo) {
	    let x = pointTo.x - pointFrom.x;
	    let y = pointTo.y - pointFrom.y;
	  
	    let angle = Math.atan2(x, y);
	    let dAngle = this.toDegree(angle);
	    
	    return dAngle;
	  }

	  calcArrowPosition(pointTo,pointFrom, dir, m) {
	    let arrowSize = this.style.connection.arrowSize;
	    
	    switch(dir) {
	      case 0:
	        return {x: pointTo.x + (arrowSize / 2), y : pointTo.y + (m * (arrowSize / 2))}
	        break;
	      case 1:{
	        if(m == Infinity) {
	          return {x: pointTo.x , y : pointTo.y + (arrowSize / 2)}
	        }
	        let x = ((pointTo.y + (arrowSize / 2)) - pointFrom.y + m * pointFrom.x) / m;
	        return {x: x  , y : pointTo.y + (arrowSize / 2)}
	      }break;
	      case 2:
	      return {x: pointTo.x - (arrowSize / 2), y : pointTo.y - (m * (arrowSize / 2)) }
	      break;
	      case 3:{
	        if(m == -Infinity){
	          return {x: pointTo.x , y : pointTo.y - (arrowSize / 2)}
	        }
	        let x = ((pointTo.y - (arrowSize / 2)) - pointFrom.y + m * pointFrom.x) / m;
	        return {x: x , y : pointTo.y - (arrowSize / 2)}
	      }break;
	    }
	  }

	  drawConnection(pointFrom, pointTo, color) {
	    this.ctx.beginPath();
	    this.ctx.moveTo(pointFrom.x, pointFrom.y);
	    this.ctx.lineTo(pointTo.x, pointTo.y);
	    this.ctx.strokeStyle = color;
	    this.ctx.lineWidth = this.style.connection.lineWidth;
	    this.ctx.stroke();
	  }

	  toDegree(angle) {
	    return angle / 0.01745329 ;
	  }

	  toRadian(angle) {
	    return angle * 0.01745329 ;
	  }

	  drawTriangle(cPoint, angle = 0, size = 10, color = "black") {
	    angle = this.toRadian(angle);

	    this.ctx.beginPath();
	    this.ctx.fillStyle = color;
	    
	    let fp = {x: size/2, y: 0};
	    let fpX = this.rotateX(fp, angle); 
	    let fpY = this.rotateY(fp, angle); 
	    this.ctx.moveTo(cPoint.x+ fpX, cPoint.y + fpY);
	    
	    let sp = {x: -1*size/2, y: size/2};
	    let spX = this.rotateX(sp, angle); 
	    let spY = this.rotateY(sp, angle); 
	    this.ctx.lineTo(cPoint.x + spX, cPoint.y + spY);
	    
	    let tp = {x: -1*size/2, y: -1*size/2};
	    let tpX = this.rotateX(tp, angle); 
	    let tpY = this.rotateY(tp, angle); 
	    this.ctx.lineTo(cPoint.x + tpX, cPoint.y + tpY);

	    this.ctx.lineTo(cPoint.x + fpX, cPoint.y + fpY);

	    this.ctx.fill();
	  }

	  //angle in radians
	  rotateX(point, angle) { return (point.x * Math.cos(angle)) +  
	    (point.y * -Math.sin(angle))};
	    
	  //angle in radians
	  rotateY(point, angle) { return (point.x * Math.sin(angle)) + 
	                                  point.y * Math.cos(angle)};

	}

	class Node {

	  constructor(x,y,width,height, id) {
	    this.x = x;
	    this.y = y;
	    this.width = width;
	    this.height = height;
	    this.connections = [];
	    this.hovered = false;
	    this.airplane = false;
	    this.id = id;
	    this.isStartPoint = false;
	    this.isEndPoint = false;
	  }

	}

	class NodeConnection {

	  constructor(node, connectionType) {
	    this.node = node;
	    this.type = connectionType;
	  }

	}

	function writable(value, start = noop) {
		let stop;
		const subscribers = [];

		function set(new_value) {
			if (safe_not_equal(value, new_value)) {
				value = new_value;
				if (!stop) return; // not ready
				subscribers.forEach(s => s[1]());
				subscribers.forEach(s => s[0](value));
			}
		}

		function update(fn) {
			set(fn(value));
		}

		function subscribe(run, invalidate = noop) {
			const subscriber = [run, invalidate];
			subscribers.push(subscriber);
			if (subscribers.length === 1) stop = start(set) || noop;
			run(value);

			return () => {
				const index = subscribers.indexOf(subscriber);
				if (index !== -1) subscribers.splice(index, 1);
				if (subscribers.length === 0) stop();
			};
		}

		return { set, update, subscribe };
	}

	class AirportState {

	  constructor() {
	    this.graph = {};
	    this.airplanesInQueue = [];
	  }

	}

	let AirportStateStore = writable(new AirportState());

	class AirportService {

	  constructor() {
	    this.fetchAirportState();
	  }

	  fetchAirportState() {
	    //initDummy();

	    let connection = new signalR.HubConnectionBuilder().withUrl("/airport").configureLogging(signalR.LogLevel.Information).build();
	    
	    connection.start().then(async () => {
	      let state = await connection.invoke("GetAirportState");
	      
	      
	      let newState = new AirportState();

	      //dummy queue
	      newState.airplanesInQueue.push({
	        flightNumber : "123",
	        requestTime : new Date(),
	        actionType : "take off"
	      });
	      newState.airplanesInQueue.push({
	        flightNumber : "123",
	        requestTime : new Date(),
	        actionType : "take off"
	      });
	      newState.airplanesInQueue.push({
	        flightNumber : "123",
	        requestTime : new Date(),
	        actionType : "take off"
	      });
	      newState.airplanesInQueue.push({
	        flightNumber : "123",
	        requestTime : new Date(),
	        actionType : "take off"
	      });

	      let graph = new NodeGraph();
	      
	      let nodes = this.convertNodesToClient(state);
	      console.log(nodes);
	      
	      graph.addNodes(nodes);
	      newState.graph = graph;
	      
	      AirportStateStore.set(newState);

	    });

	  }

	  convertNodesToClient(serverNodes) {
	    let nodeList = serverNodes.map(node => {
	      let newNode = new Node(node.x, node.y, node.width, node.height, node.id);
	      
	      newNode.isStartPoint = node.startPoint;
	      newNode.isEndPoint = node.endPoint;
	      
	      return newNode;
	    });
	    console.log(serverNodes);
	    
	    serverNodes.forEach((node,idx) => { 

	      if(node.nextStations) { 
	      
	        Object.keys(node.nextStations).forEach(type => {
	      
	          node.nextStations[type].forEach((tNode,tIdx) => {

	            nodeList[idx].connections.push(
	              new NodeConnection(nodeList[tNode.id-1],
	                this.actionTypeToClient(type)));

	          });
	      
	        });
	      
	      }
	      
	    });
	        

	    return nodeList;
	  }

	  actionTypeToClient(type) {
	    switch (type) {
	      case "takeoff":
	        return "AT_TAKEOFF"
	      break;
	      case "landing":
	        return "AT_LANDING"
	        break;
	    }
	  }

	}

	/* src\components\NodeCanvas.svelte generated by Svelte v3.1.0 */

	const file = "src\\components\\NodeCanvas.svelte";

	function create_fragment(ctx) {
		var div, canvas_1, dispose;

		return {
			c: function create() {
				div = element("div");
				canvas_1 = element("canvas");
				add_location(canvas_1, file, 50, 0, 1240);
				div.className = "middle svelte-1eqx4ta";
				add_location(div, file, 49, 0, 1218);

				dispose = [
					listen(canvas_1, "mousemove", ctx.onMouseMove),
					listen(canvas_1, "mousedown", ctx.mousedown_handler),
					listen(canvas_1, "mouseup", ctx.mouseup_handler)
				];
			},

			l: function claim(nodes) {
				throw new Error("options.hydrate only works if the component was compiled with the `hydratable: true` option");
			},

			m: function mount(target, anchor) {
				insert(target, div, anchor);
				append(div, canvas_1);
				add_binding_callback(() => ctx.canvas_1_binding(canvas_1, null));
			},

			p: function update(changed, ctx) {
				if (changed.items) {
					ctx.canvas_1_binding(null, canvas_1);
					ctx.canvas_1_binding(canvas_1, null);
				}
			},

			i: noop,
			o: noop,

			d: function destroy(detaching) {
				if (detaching) {
					detach(div);
				}

				ctx.canvas_1_binding(null, canvas_1);
				run_all(dispose);
			}
		};
	}

	function instance($$self, $$props, $$invalidate) {
		let $AirportStateStore;

		validate_store(AirportStateStore, 'AirportStateStore');
		subscribe($$self, AirportStateStore, $$value => { $AirportStateStore = $$value; $$invalidate('$AirportStateStore', $AirportStateStore); });

		

	  let canvas;
	  let context;
	  let renderer;


	  onMount(() => {
	    canvas.width = 800; $$invalidate('canvas', canvas);
	    canvas.height = 600; $$invalidate('canvas', canvas);
	    $$invalidate('context', context = canvas.getContext("2d"));
	    $$invalidate('renderer', renderer = new NodeGraphRenderer(context));

	    AirportStateStore.subscribe(state => {      
	      renderer.drawNodeGraph($AirportStateStore.graph);
	    });

	    renderer.drawNodeGraph($AirportStateStore.graph);

	});

	  function onMouseMove(e) {
	    let bounding = canvas.getBoundingClientRect();
	    $AirportStateStore.graph.onCursorMoved({x: e.clientX - bounding.left,
	                             y: e.clientY - bounding.top});
	    
	    renderer.drawNodeGraph($AirportStateStore.graph);
	  }

		function canvas_1_binding($$node, check) {
			canvas = $$node;
			$$invalidate('canvas', canvas);
		}

		function mousedown_handler() {
			return $AirportStateStore.graph.onCursorPressed();
		}

		function mouseup_handler() {
			return $AirportStateStore.graph.onCursorReleased();
		}

		return {
			canvas,
			onMouseMove,
			$AirportStateStore,
			canvas_1_binding,
			mousedown_handler,
			mouseup_handler
		};
	}

	class NodeCanvas extends SvelteComponentDev {
		constructor(options) {
			super(options);
			init(this, options, instance, create_fragment, safe_not_equal, []);
		}
	}

	/* src\components\List.svelte generated by Svelte v3.1.0 */

	const file$1 = "src\\components\\List.svelte";

	function get_each_context(ctx, list, i) {
		const child_ctx = Object.create(ctx);
		child_ctx.item = list[i];
		return child_ctx;
	}

	// (52:1) {#each $AirportStateStore.airplanesInQueue as item}
	function create_each_block(ctx) {
		var div, p0, t0_value = ctx.item.flightNumber, t0, t1, p1, t2_value = formatDate(ctx.item.requestTime), t2, t3, p2, t4_value = ctx.item.actionType, t4, t5;

		return {
			c: function create() {
				div = element("div");
				p0 = element("p");
				t0 = text(t0_value);
				t1 = space();
				p1 = element("p");
				t2 = text(t2_value);
				t3 = space();
				p2 = element("p");
				t4 = text(t4_value);
				t5 = space();
				p0.className = "svelte-9mq8ny";
				add_location(p0, file$1, 53, 12, 1030);
				p1.className = "svelte-9mq8ny";
				add_location(p1, file$1, 54, 12, 1070);
				p2.className = "svelte-9mq8ny";
				add_location(p2, file$1, 55, 12, 1121);
				div.className = "row svelte-9mq8ny";
				add_location(div, file$1, 52, 2, 999);
			},

			m: function mount(target, anchor) {
				insert(target, div, anchor);
				append(div, p0);
				append(p0, t0);
				append(div, t1);
				append(div, p1);
				append(p1, t2);
				append(div, t3);
				append(div, p2);
				append(p2, t4);
				append(div, t5);
			},

			p: function update(changed, ctx) {
				if ((changed.$AirportStateStore) && t0_value !== (t0_value = ctx.item.flightNumber)) {
					set_data(t0, t0_value);
				}

				if ((changed.$AirportStateStore) && t2_value !== (t2_value = formatDate(ctx.item.requestTime))) {
					set_data(t2, t2_value);
				}

				if ((changed.$AirportStateStore) && t4_value !== (t4_value = ctx.item.actionType)) {
					set_data(t4, t4_value);
				}
			},

			d: function destroy(detaching) {
				if (detaching) {
					detach(div);
				}
			}
		};
	}

	function create_fragment$1(ctx) {
		var div1, div0, p0, t1, p1, t3, p2, t5;

		var each_value = ctx.$AirportStateStore.airplanesInQueue;

		var each_blocks = [];

		for (var i = 0; i < each_value.length; i += 1) {
			each_blocks[i] = create_each_block(get_each_context(ctx, each_value, i));
		}

		return {
			c: function create() {
				div1 = element("div");
				div0 = element("div");
				p0 = element("p");
				p0.textContent = "Flight Number";
				t1 = space();
				p1 = element("p");
				p1.textContent = "Request Time";
				t3 = space();
				p2 = element("p");
				p2.textContent = "Action Type";
				t5 = space();

				for (var i = 0; i < each_blocks.length; i += 1) {
					each_blocks[i].c();
				}
				p0.className = "svelte-9mq8ny";
				add_location(p0, file$1, 47, 8, 852);
				p1.className = "svelte-9mq8ny";
				add_location(p1, file$1, 48, 8, 882);
				p2.className = "svelte-9mq8ny";
				add_location(p2, file$1, 49, 8, 911);
				div0.className = "row svelte-9mq8ny";
				add_location(div0, file$1, 46, 4, 825);
				div1.className = "list svelte-9mq8ny";
				add_location(div1, file$1, 45, 0, 801);
			},

			l: function claim(nodes) {
				throw new Error("options.hydrate only works if the component was compiled with the `hydratable: true` option");
			},

			m: function mount(target, anchor) {
				insert(target, div1, anchor);
				append(div1, div0);
				append(div0, p0);
				append(div0, t1);
				append(div0, p1);
				append(div0, t3);
				append(div0, p2);
				append(div1, t5);

				for (var i = 0; i < each_blocks.length; i += 1) {
					each_blocks[i].m(div1, null);
				}
			},

			p: function update(changed, ctx) {
				if (changed.$AirportStateStore || changed.formatDate) {
					each_value = ctx.$AirportStateStore.airplanesInQueue;

					for (var i = 0; i < each_value.length; i += 1) {
						const child_ctx = get_each_context(ctx, each_value, i);

						if (each_blocks[i]) {
							each_blocks[i].p(changed, child_ctx);
						} else {
							each_blocks[i] = create_each_block(child_ctx);
							each_blocks[i].c();
							each_blocks[i].m(div1, null);
						}
					}

					for (; i < each_blocks.length; i += 1) {
						each_blocks[i].d(1);
					}
					each_blocks.length = each_value.length;
				}
			},

			i: noop,
			o: noop,

			d: function destroy(detaching) {
				if (detaching) {
					detach(div1);
				}

				destroy_each(each_blocks, detaching);
			}
		};
	}

	function formatDate(date) {
	    return date.toLocaleString();
	}

	function instance$1($$self, $$props, $$invalidate) {
		let $AirportStateStore;

		validate_store(AirportStateStore, 'AirportStateStore');
		subscribe($$self, AirportStateStore, $$value => { $AirportStateStore = $$value; $$invalidate('$AirportStateStore', $AirportStateStore); });

		return { $AirportStateStore };
	}

	class List extends SvelteComponentDev {
		constructor(options) {
			super(options);
			init(this, options, instance$1, create_fragment$1, safe_not_equal, []);
		}
	}

	/* src\App.svelte generated by Svelte v3.1.0 */

	const file$2 = "src\\App.svelte";

	function create_fragment$2(ctx) {
		var div, h1, t1, t2, current;

		var nodecanvas = new NodeCanvas({ $$inline: true });

		var list = new List({ $$inline: true });

		return {
			c: function create() {
				div = element("div");
				h1 = element("h1");
				h1.textContent = "Airport";
				t1 = space();
				nodecanvas.$$.fragment.c();
				t2 = space();
				list.$$.fragment.c();
				h1.className = "svelte-1x8ajy6";
				add_location(h1, file$2, 20, 1, 407);
				div.className = "main svelte-1x8ajy6";
				add_location(div, file$2, 19, 0, 386);
			},

			l: function claim(nodes) {
				throw new Error("options.hydrate only works if the component was compiled with the `hydratable: true` option");
			},

			m: function mount(target, anchor) {
				insert(target, div, anchor);
				append(div, h1);
				append(div, t1);
				mount_component(nodecanvas, div, null);
				append(div, t2);
				mount_component(list, div, null);
				current = true;
			},

			p: noop,

			i: function intro(local) {
				if (current) return;
				nodecanvas.$$.fragment.i(local);

				list.$$.fragment.i(local);

				current = true;
			},

			o: function outro(local) {
				nodecanvas.$$.fragment.o(local);
				list.$$.fragment.o(local);
				current = false;
			},

			d: function destroy(detaching) {
				if (detaching) {
					detach(div);
				}

				nodecanvas.$destroy();

				list.$destroy();
			}
		};
	}

	function instance$2($$self) {
		

		let airportService = new AirportService();

		return {};
	}

	class App extends SvelteComponentDev {
		constructor(options) {
			super(options);
			init(this, options, instance$2, create_fragment$2, safe_not_equal, []);
		}
	}

	const app = new App({
		target: document.body
	});

	return app;

}());
//# sourceMappingURL=bundle.js.map
