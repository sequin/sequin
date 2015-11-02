# Sequin [![Build status](https://ci.appveyor.com/api/projects/status/558y2o7e3314d2nk/branch/master?svg=true)](https://ci.appveyor.com/project/jasonmitchell/sequin/branch/master) [![NuGet Version](http://img.shields.io/nuget/v/Sequin.svg?style=flat)](https://www.nuget.org/packages/Sequin/)

OWIN middleware for creating a customisable command processing pipeline in .NET CQRS applications.

## Quickstart

### Installation

Install from NuGet:

```Install-Package Sequin```

Add/update your Startup.cs class and configure Sequin:

```csharp
public class Startup
{
    public void Configuration(IAppBuilder app)
    {
        app.UseSequin();
    }
}
```

The default configuration will configure a ```/commands``` endpoint to handle commands and register all command handlers 
in referenced assemblies.

### Create a command handler

Commands are simple classes which define properties to hold the command data:

```csharp
public class SampleCommand
{
    public int PropertyA { get; set; }
    public int PropertyB { get; set; }
}
```

Command handlers are classes which implement the ```IHandler<T>``` interface defined in the the ```Sequin.Core``` package:

```csharp
public class SampleCommandHandler : IHandler<SampleCommand>
{
    public void Handle(SampleCommand command)
    {
        // Handler implementation
    }
}
```

The default Sequin configuration will automatically discover classes implementing ```IHandler``` in referenced assemblies.

### Issue commands

Commands are issued as JSON PUT requests to the configured endpoint URL (by default this is ```/commands```).  Commands
are identified in a request by setting a header of ```command``` on the request; the header value must match the name of
a command class name.

### Configuring the Sequin pipeline

Sequin enables any piece of OWIN middleware to be plugged in between a command being discovered in a request and it
being issued to the appropriate handler.  This allows for functionality such as authentication and validation to be
hooked in.

```csharp
app.UseSequin(new SequinOptions
{
    CommandPipeline = new []
    {
        new CommandPipelineStage(typeof(AuthorizeCommand)), 
        new CommandPipelineStage(typeof(MyCustomMiddleware)),
    }
});
```

## Updates

Releases will be pushed regularly to [NuGet](https://nuget.org/packages/sequin).  A new release is created automatically
whenever changes are pushed to ```master```.