![odinLZ4](https://user-images.githubusercontent.com/29813954/111915103-582fba00-8a7d-11eb-865d-1bd6b880bece.png)
# OdinLZ4Extension

The fastest and most efficient binary compression solution for [Odin](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)

## Navigation

- [OdinLZ4Extension](#odinlz4extension)
  - [Navigation](#navigation)
  - [About](#about)
    - [What's under the hood](#whats-under-the-hood)
  - [Roadmap](#roadmap)
  - [Compatibility](#compatibility)
  - [Dependencies](#dependencies)
  - [How to use](#how-to-use)
    - [Preparation](#preparation)
    - [Serialization](#serialization)
      - [Base Serialization](#base-serialization)
      - [Lazy Serialization](#lazy-serialization)
      - [Serialization with Unity object references](#serialization-with-unity-object-references)
      - [Lazy Serialization with Unity object references](#lazy-serialization-with-unity-object-references)
    - [Deserialization](#deserialization)
      - [Base Deserialization](#base-deserialization)
      - [Lazy Deserialization](#lazy-deserialization)
      - [Deserialization with Unity object references](#deserialization-with-unity-object-references)
      - [Lazy Deserialization with Unity object references](#lazy-deserialization-with-unity-object-references)
    - [Compression levels](#compression-levels)
    - [Examples](#examples)
  - [How to install System libraries for LZ4](#how-to-install-system-libraries-for-lz4)
  - [How to help the project](#how-to-help-the-project)
  - [Contanct](#contanct)

## About

[Odin - Inspector and Serializer](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) is one of the most popular development tools for Unity,
its power and functionality is incredible, one of the main functions is de/serialization of data,
it is extremely important function for most who use "Odin".

If you use cloud solutions in your projects and store some data in a serialized form,
then imagine how much data space you can save with fast and efficient compression.

The best available algorithm was chosen as the compression engine, this is [LZ4](https://en.wikipedia.org/wiki/LZ4_(compression_algorithm)), it is extremely fast and efficient.

Combining these two engineering wonders we get extremely fast de/serialization and more free space for data.

So, Odin + LZ4 = PROFIT :muscle:

### What's under the hood

The best [realization](https://github.com/MiloszKrajewski/K4os.Compression.LZ4) of the LZ4 algorithm. This solution is provided by Milos Krajewski under the MIT license.

Original LZ4 has been written by Yann Collet and original 'C' sources can be found [here](https://github.com/lz4/lz4)

More info [here](http://lz4.github.io/lz4)

>If you find this project useful, star it, I will be grateful.

## Roadmap

|  Status   | Milestone                                                                     |
| :-------: | :---------------------------------------------------------------------------- |
| :rocket:  | Implement features that are available in the `SerializationUtility` using LZ4 |
| :pushpin: | Cut out the main parts of the LZ4 engine and compile a lightweight `.dll`     |
| :pushpin: | Multithread de/compressing and de/serializing                                 |
| :pushpin: | De/Serialization performance tests with and without compression               |

## Compatibility

| Backend | .Net  | Odin  |     Compatible     |
| :-----: | :---: | :---: | :----------------: |
| IL2CPP  |  4.x  |  3.x  | :white_check_mark: |

## Dependencies

- [Odin](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)
- System libraries
  - [System.Memory](https://www.nuget.org/packages/System.Memory/)
  - [System.Buffers](https://www.nuget.org/packages/System.Buffers/)
  - [System.Runtime.CompilerServices.Unsafe](https://www.nuget.org/packages/System.Runtime.CompilerServices.Unsafe/)

> The `OdinLZ4Extension` does not contain any source materials of the `Sirenix`, you can get `Odin - Inspector and Serializer` [on the AssetStore](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)

>At the moment `OdinLZ4Extension` only supports [Odin - Inspector and Serializer](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)  
If you are only using the [Odin Serializer](https://odininspector.com/odin-serializer) then this extension will not be successfully imported into the Unity project. The reason for this is different `dll`s and namespaces. If you are using only [Odin Serializer](https://odininspector.com/odin-serializer) and would like to have this extension, [write me](#contanct) and I will add this point to the [roadmap](#roadmap).

## How to use

### Preparation

1. Include LZ4 necessary libraries ([how to do it](#how-to-install-system-libraries-for-lz4))

2. Add the OdinLZ4Extension to your project, you can do it in the following way:
   - import [latest](https://github.com/inc8877/OdinLZ4Extension/releases) extension version via [UPM](https://docs.unity3d.com/Packages/com.unity.package-manager-ui@2.0/manual/index.html)
   - add extension as a [submodule](https://git-scm.com/book/en/v2/Git-Tools-Submodules)
   - import source code

3. Plugin namespaces

```c#
using K4os.Compression.LZ4;
using OdinLZ4Extension;
```

### Serialization

Several methods are provided for serialization with subsequent compression.

#### Base Serialization

```c#
SerializeValue<T>(T value, DataFormat format, SerializationContext ctx = null, OdinLZ4Level level = OdinLZ4Level.FAST)
```

---

#### Lazy Serialization

Fast serialization in binary format.

```c#
LazySerialization<T>(T value, OdinLZ4Level level = OdinLZ4Level.FAST)
```

---

#### Serialization with Unity object references

```c#
SerializeValue<T>(T value, DataFormat format, out List<UnityEngine.Object> unityObjects, SerializationContext ctx = null, OdinLZ4Level level = OdinLZ4Level.FAST)
```

---

#### Lazy Serialization with Unity object references

```c#
LazySerialization<T>(T value, out List<UnityEngine.Object> unityObjects, OdinLZ4Level level = OdinLZ4Level.FAST)
```

### Deserialization

Several convenient methods are provided for deserializing compressed objects.

#### Base Deserialization

```c#
TResult DeserializeValue<TResult>(byte[] bytes, DataFormat format, DeserializationContext ctx = null)
```

---

#### Lazy Deserialization

```c#
TResult LazyDeserialization<TResult>(byte[] bytes)
```

---

#### Deserialization with Unity object references

Decompresses and deserializes a value of a given type from the given byte array in the given format, using the given list of Unity objects for external index reference resolution

```c#
TResult DeserializeValue<TResult>(byte[] bytes, DataFormat format, List<UnityEngine.Object> referencedUnityObjects, DeserializationContext ctx = null)
```

---

#### Lazy Deserialization with Unity object references

Fast data deserialization from binary format, using the given list of Unity objects for external index reference resolution

```c#
TResult LazyDeserialization<TResult>(byte[] bytes, List<UnityEngine.Object> referencedUnityObjects)
```

---

### Compression levels

When you compress serialized data, `FAST` compression is applied by default. You can choose the compression level for your needs.  
Available compression levels:

- `FAST` (maximum compression speed)
- `OPTIMAL` (sweet spot, fast and efficient compression)
- `MAX` (slower than others but most efficient)

Code example:

```c#
OdinLZ4API.SerializeValue(SERIALIZABLE_VALUE, DataFormat.Binary, level: OdinLZ4Level.OPTIMAL);
OdinLZ4API.LazySerialization(SERIALIZABLE_VALUE, OdinLZ4Level.MAX);
```

### Examples

Code examples:

```c#
[Serializable] public class SimpleData { public int a; public string someData; }
[Serializable] public class RefersData { public GameObject player1, player2; public Vector3 somePos; }

[SerializeField] private SimpleData simpleData = new SimpleData() { a = 100, someData = "It's work!" };
[SerializeField] private RefersData refersData;

SimpleData deserializedSimpleData;
RefersData deserializedRefersData;

byte[] smplSerialization, refersSerialization;
List<UnityEngine.Object> refs;

// --- Serialization ---

// Base serialization
smplSerialization = OdinLZ4API.SerializeValue(simpleData, DataFormat.Binary, level:OdinLZ4Level.MAX);
// Lazy Serialization
smplSerialization = OdinLZ4API.LazySerialization(simpleData);

// Serialization with Unity object references
refersSerialization = OdinLZ4API.SerializeValue(refersData, DataFormat.Binary, out refs);
// Lazy Serialization with Unity object references
refersSerialization = OdinLZ4API.LazySerialization(refersData, out refs);

// --- Derialization ---

// Base deserialization
deserializedSimpleData = OdinLZ4API.DeserializeValue<SimpleData>(smplSerialization, DataFormat.Binary);
// Lazy deserialization
deserializedSimpleData = OdinLZ4API.LazyDeserialization<SimpleData>(smplSerialization);

// Deserialization using the given list of Unity objects for external index reference resolution
deserializedRefersData = OdinLZ4API.DeserializeValue<RefersData>(refersSerialization, DataFormat.Binary, refs);
// Lazy deserialization using the given list of Unity objects for external index reference resolution
deserializedRefersData = OdinLZ4API.LazyDeserialization<RefersData>(refersSerialization, refs);
```

## How to install System libraries for LZ4

Unity does not provide the necessary libraries for the LZ4, so you need to install them manually.
The libraries can be downloaded via NuGet, but since the Unity does not work well with it, you need to get the `.dll` from the downloaded packages and put them into the `Assets` folder.

If you don't want to bother with it then you can download the necessary `.dll` that are available in each [Release](https://github.com/inc8877/OdinLZ4Extension/releases)

A good location for `.dll` in the project would be the `Assets/Plugins` folder.

## How to help the project

At the moment the project has just begun its life and requires a lot of work,
if you are reading this means you may have liked this project and you can help by doing the following:

- do performance tests (compare data serialization with and without compression and make a graph)
- expand functionality:
  - serialization with compressing via multithreading
  - a pool of serialization tasks with callbacks

## Contanct

email: <inc8877@gmail.com>
