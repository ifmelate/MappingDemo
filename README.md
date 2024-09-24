# Comparing Mapping Approaches in .NET

## Two Common Approaches to Model/DTO Mapping

1. Manual mapping - the developer fully manages/controls the process of data transfer between models
2. Auto-mapping - the developer uses a ready-made library that does the mapping automatically according to the developer's instructions


## Drawbacks of Manual Mapping

- Time spent on mapping essentially identical fields in models
- A lot of control is needed everywhere to check what you forgot to map - the compiler doesn't prompt, you need to cover such cases with tests.


## Issues with **AutoMapper**

For the second approach in .NET, the most popular library, **AutoMapper**, is usually used. It seems fine, you don't have to think about field control as with manual mapping, you add to both models and it's automatically mapped. But not everything is good here either:

- I'm not shown where my fields in the model are used, because the mapping happens in the depths of **AutoMapper**, so we learn about problems at runtime or by calling AssertConfigurationIsValid at application startup, or in unit tests
- I need to inject **AutoMapper** into services (classes) using the IMapper interface. And still, to find all the places in the code where my models are mapped, I need to search in the IDE for something like Map`<SomeDto>` or Map`<List<SomeDto>`>, etc.
- With **AutoMapper**, bugs with mapping were observed on projects directly in environments, especially bugs related to nullable fields. If nullable fields weren't handled properly somewhere, that's it. Example: requested the first 100 elements by pagination - it works, but on the next page - an error (c# object reference not set to an instance).


As communication with colleagues on projects shows, distrust in the mapping library is the reason for continuing to use manual mapping in projects.

## Introducing **Mapperly**

There is a better alternative - a mapping library based on source generators
And the name of this library is **Mapperly** (https://github.com/riok/mapperly)

This library solves the problems of **AutoMapper** and doesn't exclude the use of manual mapping if you really want to :)

### Advantages of **Mapperly**:

- Ease of use
- Ability to find where a field is used in mapping, thanks to source generators
- Faster performance, again, thanks to source generators
- You can combine both auto-mapping and manual mapping within one model
- More intelligently able to map types automatically
- No need to inject anything into classes


## Setting Up **Mapperly**

1. Install the Riok.**Mapperly** package

```plaintext
dotnet add package Riok.Mapperly
```


2. Create a class (preferably static) and set the [Mapper] attribute on it
<img width="326" alt="1" src="https://github.com/user-attachments/assets/16e44a7d-23ce-44a6-82c4-7bffd2938e1e">


3. We write a method as an extension to our model:
<img width="812" alt="2" src="https://github.com/user-attachments/assets/759cc423-0202-4083-9cb3-976b77b3e043">

4. We specify with attributes additionally what needs to be mapped if the fields don't match by name
<img width="891" alt="3" src="https://github.com/user-attachments/assets/c6bf7e56-29bd-4e94-af38-5fef656ac572">

In **AutoMapper**, the same would look like this:
<img width="818" alt="4" src="https://github.com/user-attachments/assets/2b3fd531-c702-418c-ac0c-1f9f21e6bb71">

Notice, I had to help **AutoMapper** to map datetime->dateonly, but **Mapperly** can do this. Let's look at the generated code:
![5](https://github.com/user-attachments/assets/e3da18c8-f45d-4669-8a14-e99b36a25acb)

Now you can go to the method implementation in the code through the Go to Declaration or Usages call, or go through the solution explorer as in the screenshot on the left.

And most importantly, what we got, we can now see where our fields are used in both models
<img width="912" alt="6" src="https://github.com/user-attachments/assets/7e5dabe8-ec96-42be-8e5c-a10dce992a85">
<img width="949" alt="7" src="https://github.com/user-attachments/assets/90d4baa8-32bc-4b83-bcc9-aa31d21e6c7f">

And we also have the ability to find where this method is used in the code:
<img width="882" alt="8" src="https://github.com/user-attachments/assets/ea3c31f1-e148-4a6d-8538-45b3dcce35fe">

### Let's now try to consider more custom scenarios:
1. Ignore a specific field:
<img width="931" alt="9" src="https://github.com/user-attachments/assets/fb1c0015-315f-4049-b31d-9387cda3f4ed">

2. Apply custom mapping to a separate field:
<img width="916" alt="10" src="https://github.com/user-attachments/assets/ad080cfc-4f23-4664-b47c-7dbdfbd328b9">

IMPORTANT: MapToVisitsSummary uses => lambda. This is important for the mapping translation to work in a DB query. Always use lambda in **Mapperly**, not a return statement.

In **AutoMapper**, we would implement the example like this:
<img width="954" alt="11" src="https://github.com/user-attachments/assets/74ab432f-ecba-49a6-937e-1149fa80171d">

3. And how does Projection work there, you ask?
It works fine, but you need to write one more method, which, by the way, is easier to track later:
<img width="961" alt="12" src="https://github.com/user-attachments/assets/b6c6b982-c31f-45e4-9251-fdac92f7635f">

This code is generated:
![13](https://github.com/user-attachments/assets/09c2211b-4fc8-4476-908d-23dd7be0b40e)

on line 49, due to that => lambda, the linq is built correctly

4. Let's try to combine auto-mapping with manual mapping
![14](https://github.com/user-attachments/assets/a091862c-75b1-4224-ad37-5b2467e3489b)

Generated code:
<img width="948" alt="15_Correct" src="https://github.com/user-attachments/assets/645a31ce-5307-4225-a4a2-40b46a649f39">

5. Mapping using base models

Suppose we introduce a base model for entity: two matching fields Id and UserId + isDeleted field
<img width="399" alt="16" src="https://github.com/user-attachments/assets/e5feb030-15c5-4d18-aa8a-a312ce80b719">

And we introduce a base model for dto with a custom Status field
<img width="369" alt="17" src="https://github.com/user-attachments/assets/01552959-0c9b-4e18-a0f7-667fd678dd11">

We inherited our test model from these base ones:
<img width="937" alt="18" src="https://github.com/user-attachments/assets/5b3b4ba1-b131-4014-b61a-6657475b83a5">

And now we want to make the mapping continue to work.
In **AutoMapper**, we need to give instructions on the base model:
<img width="797" alt="19" src="https://github.com/user-attachments/assets/2e380090-61a2-4dab-b0bd-41d3ae6f7a3f">

In **Mapperly**, this case looks more complicated, in this aspect it is inferior to **AutoMapper**:
<img width="926" alt="20" src="https://github.com/user-attachments/assets/6055a1e8-155b-47b2-a430-90b74faf4bce">

You have to specify for each inherited type how to map the custom Status field in BaseDto. The Id and UserId fields mapped without problems.
For now, it works like this, there's an open ticket in the library's GitHub repository to improve work with derived types.
Despite this drawback, it's easier to keep track of unmapped fields in **Mapperly** than in **AutoMapper**, and even easier than with manual mapping

This is achieved by adding lines to the .editorconfig file of the project:

```plaintext
[*.cs]
dotnet_diagnostic.RMG012.severity = error # Unmapped target member
```

Now, if I add a new field in Dto with a name or type that **Mapperly** can't map on the fly, I'll get an error at the project compilation stage:
![21](https://github.com/user-attachments/assets/a3ccecff-4923-4bc1-8df0-1eb8f8c1c416)

Actually, this feature will allow you not to have problems with the fact that you didn't keep track of mapping somewhere when adding new fields to the model.


That's all:)
I hope the information was useful and you will try to use it in your projects!
